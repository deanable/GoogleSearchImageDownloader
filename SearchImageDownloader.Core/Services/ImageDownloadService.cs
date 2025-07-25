using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SearchImageDownloader.Core.Models;
using SearchImageDownloader.Core.Interfaces;

namespace SearchImageDownloader.Core.Services
{
    public class ImageDownloadService : IImageDownloadService
    {
        public async Task DownloadImagesAsync(IEnumerable<ImageResult> images, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
            int count = 1;
            using (var http = new HttpClient())
            {
                foreach (var img in images)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(img.ImageUrl)) continue;
                        var ext = Path.GetExtension(new Uri(img.ImageUrl).AbsolutePath);
                        if (string.IsNullOrEmpty(ext) || ext.Length > 5) ext = ".jpg";
                        // Sanitize title for filename
                        var title = string.IsNullOrWhiteSpace(img.Title) ? "image" : img.Title;
                        foreach (var c in Path.GetInvalidFileNameChars())
                            title = title.Replace(c, '_');
                        if (title.Length > 40) title = title.Substring(0, 40);
                        var fileName = $"{title}_{count}{ext}";
                        var filePath = Path.Combine(targetFolder, fileName);
                        int uniqueIdx = 1;
                        while (File.Exists(filePath))
                        {
                            fileName = $"{title}_{count}_{uniqueIdx}{ext}";
                            filePath = Path.Combine(targetFolder, fileName);
                            uniqueIdx++;
                        }
                        var data = await http.GetByteArrayAsync(img.ImageUrl);
                        await File.WriteAllBytesAsync(filePath, data);
                        count++;
                    }
                    catch { /* Optionally log or handle errors */ }
                }
            }
        }
    }
} 