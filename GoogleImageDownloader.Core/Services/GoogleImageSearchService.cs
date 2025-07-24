using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GoogleImageDownloader.Core.Services
{
    public class GoogleImageSearchService : IGoogleImageSearchService
    {
        public IEnumerable<ImageResult> SearchImages(SearchFilters filters)
        {
            // For now, call the async version and block (WinForms is not async-friendly by default)
            return SearchImagesAsync(filters).GetAwaiter().GetResult();
        }

        private async Task<IEnumerable<ImageResult>> SearchImagesAsync(SearchFilters filters)
        {
            var url = BuildGoogleImagesUrl(filters);
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                var html = await http.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var results = new List<ImageResult>();
                // Google Images results are in <img> tags with class 'rg_i Q4LuWd' or similar
                var imgNodes = doc.DocumentNode.SelectNodes("//img[contains(@class, 'rg_i')]");
                if (imgNodes != null)
                {
                    foreach (var img in imgNodes)
                    {
                        var imgUrl = img.GetAttributeValue("data-src", null) ?? img.GetAttributeValue("src", null);
                        if (string.IsNullOrEmpty(imgUrl) || imgUrl.StartsWith("data:"))
                            continue; // skip base64 or empty
                        var title = img.GetAttributeValue("alt", "");
                        // Try to get the parent <a> for the source page
                        var parentA = img.ParentNode;
                        string sourcePage = null;
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
                    }
                }
                return results;
            }
        }

        private string BuildGoogleImagesUrl(SearchFilters filters)
        {
            // Build the Google Images search URL with filters
            var baseUrl = "https://www.google.com/search?tbm=isch";
            var query = System.Web.HttpUtility.UrlEncode(filters.Query ?? "");
            var url = $"{baseUrl}&q={query}";
            // Add filters as tbs parameters
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
            return url;
        }
    }
} 