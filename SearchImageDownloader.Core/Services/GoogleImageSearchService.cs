using System;
using System.Collections.Generic;
using SearchImageDownloader.Core.Models;
using SearchImageDownloader.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;

namespace SearchImageDownloader.Core.Services
{
    public static class CoreLogger
    {
        private static string? logFilePath;
        private static readonly BlockingCollection<string> logQueue = new();
        private static Thread? logThread;
        private static bool isRunning = false;

        public static void Init(string path)
        {
            logFilePath = path;
            File.AppendAllText(logFilePath, $"[CoreLogger] Initialized at {DateTime.Now}\r\n");
            if (!isRunning)
            {
                isRunning = true;
                logThread = new Thread(ProcessLogQueue) { IsBackground = true };
                logThread.Start();
            }
        }

        public static void Log(string message)
        {
            if (logFilePath == null) return;
            try
            {
                logQueue.Add($"[{DateTime.Now:HH:mm:ss}] [Core] {message}\r\n");
            }
            catch (Exception)
            {
                // Ignore logging errors
            }
        }

        private static void ProcessLogQueue()
        {
            while (isRunning)
            {
                try
                {
                    var entry = logQueue.Take();
                    if (logFilePath != null)
                    {
                        bool written = false;
                        int retries = 5;
                        while (!written && retries-- > 0)
                        {
                            try
                            {
                                using (var fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                                using (var sw = new StreamWriter(fs))
                                {
                                    sw.Write(entry);
                                }
                                written = true;
                            }
                            catch (IOException)
                            {
                                Thread.Sleep(50); // Wait and retry if file is locked
                            }
                        }
                        // If not written after retries, skip this entry
                    }
                }
                catch (InvalidOperationException)
                {
                    // logQueue was completed
                    break;
                }
                catch (Exception)
                {
                    // Ignore file write errors
                }
            }
        }

        public static void Shutdown()
        {
            isRunning = false;
            logQueue.CompleteAdding();
            logThread?.Join();
        }
    }

    public class GoogleImageSearchService : IImageSearchService
    {
        private readonly IRegistryService _registryService;
        public GoogleImageSearchService(IRegistryService registryService)
        {
            _registryService = registryService;
        }

        public IEnumerable<ImageResult> SearchImages(SearchFilters filters)
        {
            var (apiKey, cseId) = _registryService.LoadGoogleApiCredentials();
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(cseId))
                throw new InvalidOperationException("Google API Key or CSE ID is not set in the registry.");
            return SearchImagesAsync(filters, apiKey, cseId).GetAwaiter().GetResult().Results;
        }

        public (IEnumerable<ImageResult> Results, int TotalResults) SearchImagesWithTotal(SearchFilters filters)
        {
            var (apiKey, cseId) = _registryService.LoadGoogleApiCredentials();
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(cseId))
                throw new InvalidOperationException("Google API Key or CSE ID is not set in the registry.");
            return SearchImagesAsync(filters, apiKey, cseId).GetAwaiter().GetResult();
        }

        private async Task<(IEnumerable<ImageResult> Results, int TotalResults)> SearchImagesAsync(SearchFilters filters, string apiKey, string cseId)
        {
            var results = new List<ImageResult>();
            int totalResults = 0;
            try
            {
                CoreLogger.Log($"SearchImagesAsync called with filters: Query='{filters.Query}', Size='{filters.Size}', Color='{filters.Color}', UsageRights='{filters.UsageRights}', Type='{filters.Type}', Time='{filters.Time}', Start={filters.Start}, Num={filters.Num}, MinFileSizeBytes={filters.MinFileSizeBytes}");
                var url = BuildGoogleCustomSearchUrl(filters, apiKey, cseId);
                CoreLogger.Log($"Google Custom Search API URL: {url}");
                using var http = new HttpClient();
                var response = await http.GetAsync(url);
                CoreLogger.Log($"HTTP status: {(int)response.StatusCode} {response.ReasonPhrase}");
                var json = await response.Content.ReadAsStringAsync();
                CoreLogger.Log($"JSON response (first 2048 chars):\n{json.Substring(0, Math.Min(2048, json.Length))}");
                if (!response.IsSuccessStatusCode)
                {
                    CoreLogger.Log($"Google API error: {response.StatusCode} {response.ReasonPhrase}");
                    if (json.Contains("dailyLimitExceeded") || json.Contains("quotaExceeded"))
                        CoreLogger.Log("Google API quota exceeded or daily limit reached.");
                    if (json.Contains("keyInvalid") || json.Contains("invalid"))
                        CoreLogger.Log("Google API key or CSE ID invalid.");
                    return (results, 0);
                }
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("searchInformation", out var info) && info.TryGetProperty("totalResults", out var total))
                {
                    int.TryParse(total.GetString(), out totalResults);
                }
                if (root.TryGetProperty("items", out var items))
                {
                    int count = 0;
                    foreach (var item in items.EnumerateArray())
                    {
                        try
                        {
                            var link = item.GetProperty("link").GetString();
                            var title = item.GetProperty("title").GetString();
                            var image = item.GetProperty("image");
                            var thumbnail = image.TryGetProperty("thumbnailLink", out var thumbProp) ? thumbProp.GetString() : null;
                            var context = image.TryGetProperty("contextLink", out var ctxProp) ? ctxProp.GetString() : null;
                            int? byteSize = image.TryGetProperty("byteSize", out var byteSizeProp) ? byteSizeProp.GetInt32() : (int?)null;
                            if (filters.MinFileSizeBytes.HasValue && byteSize.HasValue && byteSize.Value < filters.MinFileSizeBytes.Value)
                                continue; // Skip images below min size
                            results.Add(new ImageResult
                            {
                                ImageUrl = link,
                                ThumbnailUrl = thumbnail,
                                Title = title,
                                SourcePage = context
                            });
                            count++;
                        }
                        catch (Exception ex)
                        {
                            CoreLogger.Log($"Error parsing item: {ex.Message}\n{ex.StackTrace}");
                        }
                    }
                    CoreLogger.Log($"Parsed {count} image results from API response.");
                }
                else
                {
                    CoreLogger.Log("No items found in Google API response.");
                }
            }
            catch (Exception ex)
            {
                CoreLogger.Log($"Exception in SearchImagesAsync: {ex.Message}\n{ex.StackTrace}");
            }
            return (results, totalResults);
        }

        private string BuildGoogleCustomSearchUrl(SearchFilters filters, string apiKey, string cseId)
        {
            var url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cseId}&searchType=image&q={Uri.EscapeDataString(filters.Query ?? "")}";
            url += $"&num={filters.Num}&start={filters.Start}";
            if (!string.IsNullOrEmpty(filters.Size) && filters.Size != "Any size")
            {
                // Google API: imgSize=icon|medium|large|xlarge|xxlarge|huge
                var sizeMap = new Dictionary<string, string> { { "Icon", "icon" }, { "Medium", "medium" }, { "Large", "large" }, { "X-Large", "xlarge" }, { "XX-Large", "xxlarge" }, { "Huge", "huge" } };
                if (sizeMap.TryGetValue(filters.Size, out var apiSize))
                    url += $"&imgSize={apiSize}";
            }
            if (!string.IsNullOrEmpty(filters.Color) && filters.Color != "Any color")
            {
                // Google API: imgColorType=color|gray|mono|trans
                var colorMap = new Dictionary<string, string> { { "Color", "color" }, { "Black and white", "gray" }, { "Transparent", "trans" } };
                if (colorMap.TryGetValue(filters.Color, out var apiColor))
                    url += $"&imgColorType={apiColor}";
            }
            if (!string.IsNullOrEmpty(filters.Type) && filters.Type != "Any type")
            {
                // Google API: imgType=clipart|face|lineart|news|photo
                var typeMap = new Dictionary<string, string> { { "Photo", "photo" }, { "Clipart", "clipart" }, { "Lineart", "lineart" }, { "Face", "face" }, { "News", "news" } };
                if (typeMap.TryGetValue(filters.Type, out var apiType))
                    url += $"&imgType={apiType}";
            }
            if (!string.IsNullOrEmpty(filters.UsageRights) && filters.UsageRights != "Any rights")
            {
                // Google API: rights=cc_publicdomain|cc_attribute|cc_sharealike|cc_noncommercial|cc_nonderived
                var rightsMap = new Dictionary<string, string> { { "Labeled for reuse", "cc_attribute" } };
                if (rightsMap.TryGetValue(filters.UsageRights, out var apiRights))
                    url += $"&rights={apiRights}";
            }
            if (!string.IsNullOrEmpty(filters.Time) && filters.Time != "Any time")
            {
                // Google API: dateRestrict=d[number] (d=days, w=weeks, m=months, y=years)
                var timeMap = new Dictionary<string, string> { { "Past 24 hours", "d1" }, { "Past week", "w1" }, { "Past month", "m1" }, { "Past year", "y1" } };
                if (timeMap.TryGetValue(filters.Time, out var apiTime))
                    url += $"&dateRestrict={apiTime}";
            }
            return url;
        }
    }
} 