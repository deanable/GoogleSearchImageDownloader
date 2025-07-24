using System;
using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Linq;

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
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
                    http.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
                    CoreLogger.Log("Sending HTTP GET request with desktop headers...");
                    var html = await http.GetStringAsync(url);
                    CoreLogger.Log($"Fetched HTML. Length: {html.Length}");
                    CoreLogger.Log($"HTML Preview: >>>\n{html.Substring(0, Math.Min(100000, html.Length))}\n<<< END HTML Preview");
                    // Save full HTML to a file for manual inspection
                    var htmlDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    if (!Directory.Exists(htmlDir)) Directory.CreateDirectory(htmlDir);
                    var htmlFilePath = Path.Combine(htmlDir, $"html_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                    File.WriteAllText(htmlFilePath, html);
                    CoreLogger.Log($"[DEBUG] Full HTML saved to: {htmlFilePath}");
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    CoreLogger.Log("HTML loaded into HtmlAgilityPack.");
                    CoreLogger.Log("[DEBUG] Entering advanced <div> class logging block.");
                    var divs = doc.DocumentNode.SelectNodes("//div[@class]");
                    if (divs != null)
                    {
                        var classCount = new Dictionary<string, int>();
                        foreach (var div in divs)
                        {
                            var cls = div.GetAttributeValue("class", "").Trim();
                            if (!string.IsNullOrWhiteSpace(cls))
                            {
                                if (!classCount.ContainsKey(cls))
                                    classCount[cls] = 0;
                                classCount[cls]++;
                            }
                        }
                        var topClasses = classCount.OrderByDescending(kv => kv.Value).Take(100).ToList();
                        CoreLogger.Log($"Top <div> class names (count): {string.Join(", ", topClasses.Select(kv => $"{kv.Key} ({kv.Value})"))}");
                        // Log first 5 <div> class names containing <img>
                        int found = 0;
                        foreach (var div in divs)
                        {
                            if (div.SelectSingleNode(".//img") != null)
                            {
                                var cls = div.GetAttributeValue("class", "").Trim();
                                CoreLogger.Log($"First 5 <div> class names containing <img>: {cls}");
                                found++;
                                if (found >= 5) break;
                            }
                        }
                        CoreLogger.Log("[DEBUG] Exiting advanced <div> class logging block.");
                    }
                    else
                    {
                        CoreLogger.Log("[DEBUG] No <div> elements with class found in HTML (divs == null)");
                    }
                    var results = new List<ImageResult>();
                    CoreLogger.Log("[DEBUG] Attempting to select islrc grid container...");
                    var grid = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'islrc')]");
                    if (grid != null)
                    {
                        CoreLogger.Log($"[DEBUG] islrc grid found. OuterHtml (first 500 chars): {grid.OuterHtml.Substring(0, Math.Min(500, grid.OuterHtml.Length))}");
                        var imageDivs = grid.SelectNodes(".//div[contains(@class, 'isv-r')]");
                        if (imageDivs == null || imageDivs.Count == 0)
                        {
                            CoreLogger.Log("[DEBUG] No isv-r image blocks found in islrc grid. Selector may need adjustment.");
                        }
                        else
                        {
                            CoreLogger.Log($"[DEBUG] isv-r image blocks found: {imageDivs.Count}");
                        }
                        if (imageDivs != null)
                        {
                            int imgIdx = 0;
                            foreach (var div in imageDivs)
                            {
                                if (imgIdx == 0)
                                {
                                    // Log the raw HTML of the first isv-r block for template refinement
                                    CoreLogger.Log($"[DEBUG] First isv-r block HTML: {div.OuterHtml.Substring(0, Math.Min(2000, div.OuterHtml.Length))}");
                                }
                                string? thumbUrl = null;
                                string? highResUrl = null;
                                string? sourcePage = null;
                                string? title = null;
                                // 1. Get thumbnail from first <img> (skip gif)
                                var imgNode = div.SelectSingleNode(".//img[not(starts-with(@src, 'data:image/gif'))]");
                                if (imgNode != null)
                                {
                                    thumbUrl = imgNode.GetAttributeValue("src", null);
                                    title = imgNode.GetAttributeValue("alt", "");
                                }
                                // 2. Try to get high-res and source from <script> JSON
                                var scriptNode = div.SelectSingleNode(".//script");
                                if (scriptNode != null && !string.IsNullOrWhiteSpace(scriptNode.InnerText))
                                {
                                    var json = scriptNode.InnerText;
                                    try
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(json, @"""ou"":""(.*?)""");
                                        if (match.Success)
                                            highResUrl = match.Groups[1].Value;
                                        var match2 = System.Text.RegularExpressions.Regex.Match(json, @"""ru"":""(.*?)""");
                                        if (match2.Success)
                                            sourcePage = match2.Groups[1].Value;
                                    }
                                    catch (Exception ex)
                                    {
                                        CoreLogger.Log($"[DEBUG] Exception parsing script JSON: {ex.Message}");
                                    }
                                }
                                // 3. Fallback: try data-bem or data-objurl attributes (legacy, rare)
                                if (string.IsNullOrEmpty(highResUrl))
                                {
                                    var dataBem = div.GetAttributeValue("data-bem", null);
                                    if (!string.IsNullOrEmpty(dataBem))
                                    {
                                        var match3 = System.Text.RegularExpressions.Regex.Match(dataBem, @"""img_url"":""(.*?)""");
                                        if (match3.Success)
                                            highResUrl = match3.Groups[1].Value;
                                    }
                                }
                                // 4. Fallback to thumbnail if no high-res
                                if (string.IsNullOrEmpty(highResUrl))
                                    highResUrl = thumbUrl;
                                results.Add(new ImageResult
                                {
                                    ImageUrl = highResUrl,
                                    ThumbnailUrl = thumbUrl,
                                    Title = title,
                                    SourcePage = sourcePage
                                });
                                CoreLogger.Log($"[DEBUG] Image {imgIdx}: thumb={thumbUrl}, highRes={highResUrl}, source={sourcePage}, title={title}");
                                imgIdx++;
                            }
                        }
                        CoreLogger.Log($"Total images parsed: {results.Count}");
                        return results;
                    }
                    else
                    {
                        CoreLogger.Log("[DEBUG] islrc grid not found in HTML. Selector may need adjustment.");
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