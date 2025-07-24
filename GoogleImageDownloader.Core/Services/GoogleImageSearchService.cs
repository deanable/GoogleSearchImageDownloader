using System;
using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace GoogleImageDownloader.Core.Services
{
    public static class CoreLogger
    {
        private static string? logFilePath;
        public static void Init(string path)
        {
            logFilePath = path;
            File.AppendAllText(logFilePath, $"[CoreLogger] Initialized at {DateTime.Now}\r\n");
        }
        public static void Log(string message)
        {
            if (logFilePath == null) return;
            File.AppendAllText(logFilePath, $"[{DateTime.Now:HH:mm:ss}] [Core] {message}\r\n");
        }
    }

    public class GoogleImageSearchService : IGoogleImageSearchService
    {
        public IEnumerable<ImageResult> SearchImages(SearchFilters filters)
        {
            return SearchImagesAsync(filters).GetAwaiter().GetResult();
        }

        private async Task<IEnumerable<ImageResult>> SearchImagesAsync(SearchFilters filters)
        {
            CoreLogger.Log($"SearchImagesAsync started. Query: '{filters.Query}', Filters: Size={filters.Size}, Color={filters.Color}, UsageRights={filters.UsageRights}, Type={filters.Type}, Time={filters.Time}");
            var url = BuildGoogleImagesUrl(filters);
            CoreLogger.Log($"Built Google Images URL: {url}");
            try
            {
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                    CoreLogger.Log("Sending HTTP GET request...");
                    var html = await http.GetStringAsync(url);
                    CoreLogger.Log($"Fetched HTML. Length: {html.Length}");
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    CoreLogger.Log("HTML loaded into HtmlAgilityPack.");
                    var results = new List<ImageResult>();
                    var imgNodes = doc.DocumentNode.SelectNodes("//img[contains(@class, 'rg_i')]");
                    CoreLogger.Log($"imgNodes found: {(imgNodes == null ? 0 : imgNodes.Count)}");
                    if (imgNodes != null)
                    {
                        int imgIdx = 0;
                        foreach (var img in imgNodes)
                        {
                            var imgUrl = img.GetAttributeValue("data-src", null) ?? img.GetAttributeValue("src", null);
                            if (string.IsNullOrEmpty(imgUrl) || imgUrl.StartsWith("data:"))
                            {
                                CoreLogger.Log($"Skipping image {imgIdx}: No valid URL or base64.");
                                continue;
                            }
                            var title = img.GetAttributeValue("alt", "");
                            var parentA = img.ParentNode;
                            string? sourcePage = null;
                            while (parentA != null && parentA.Name != "a")
                                parentA = parentA.ParentNode;
                            if (parentA != null && parentA.Name == "a")
                                sourcePage = parentA.GetAttributeValue("href", null);
                            results.Add(new ImageResult
                            {
                                ImageUrl = imgUrl,
                                ThumbnailUrl = imgUrl,
                                Title = title,
                                SourcePage = sourcePage
                            });
                            CoreLogger.Log($"Image {imgIdx}: URL={imgUrl}, Title={title}, Source={sourcePage}");
                            imgIdx++;
                        }
                    }
                    CoreLogger.Log($"Total images parsed: {results.Count}");
                    return results;
                }
            }
            catch (Exception ex)
            {
                CoreLogger.Log($"Exception in SearchImagesAsync: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private string BuildGoogleImagesUrl(SearchFilters filters)
        {
            var baseUrl = "https://www.google.com/search?tbm=isch";
            var query = System.Web.HttpUtility.UrlEncode(filters.Query ?? "");
            var url = $"{baseUrl}&q={query}";
            var tbs = new List<string>();
            if (!string.IsNullOrEmpty(filters.Size) && filters.Size != "Any size")
            {
                if (filters.Size == "Large") tbs.Add("isz:l");
                else if (filters.Size == "Medium") tbs.Add("isz:m");
                else if (filters.Size == "Icon") tbs.Add("isz:i");
            }
            if (!string.IsNullOrEmpty(filters.Color) && filters.Color != "Any color")
            {
                if (filters.Color == "Color") tbs.Add("ic:color");
                else if (filters.Color == "Black and white") tbs.Add("ic:gray");
                else if (filters.Color == "Transparent") tbs.Add("ic:trans");
            }
            if (!string.IsNullOrEmpty(filters.UsageRights) && filters.UsageRights != "Any rights")
            {
                if (filters.UsageRights == "Labeled for reuse") tbs.Add("sur:f");
            }
            if (!string.IsNullOrEmpty(filters.Type) && filters.Type != "Any type")
            {
                if (filters.Type == "Photo") tbs.Add("itp:photo");
                else if (filters.Type == "Clipart") tbs.Add("itp:clipart");
                else if (filters.Type == "Lineart") tbs.Add("itp:lineart");
            }
            if (!string.IsNullOrEmpty(filters.Time) && filters.Time != "Any time")
            {
                if (filters.Time == "Past 24 hours") tbs.Add("qdr:d");
                else if (filters.Time == "Past week") tbs.Add("qdr:w");
            }
            if (tbs.Count > 0)
            {
                url += "&tbs=" + string.Join(",", tbs);
            }
            CoreLogger.Log($"BuildGoogleImagesUrl: {url}");
            return url;
        }
    }
} 