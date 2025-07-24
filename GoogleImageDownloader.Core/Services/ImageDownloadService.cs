using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;

namespace GoogleImageDownloader.Core.Services
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
                        var fileName = $"image_{count}{ext}";
                        var filePath = Path.Combine(targetFolder, fileName);
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