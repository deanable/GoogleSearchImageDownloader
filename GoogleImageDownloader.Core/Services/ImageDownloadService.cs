using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;

namespace GoogleImageDownloader.Core.Services
{
    public class ImageDownloadService : IImageDownloadService
    {
        public void DownloadImages(IEnumerable<ImageResult> images, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
            int count = 1;
            foreach (var img in images)
            {
                try
                {
                    var ext = Path.GetExtension(new Uri(img.ImageUrl).AbsolutePath);
                    if (string.IsNullOrEmpty(ext) || ext.Length > 5) ext = ".jpg";
                    var fileName = $"image_{count}{ext}";
                    var filePath = Path.Combine(targetFolder, fileName);
                    using (var wc = new WebClient())
                    {
                        wc.DownloadFile(img.ImageUrl, filePath);
                    }
                    count++;
                }
                catch { /* Optionally log or handle errors */ }
            }
        }
    }
} 