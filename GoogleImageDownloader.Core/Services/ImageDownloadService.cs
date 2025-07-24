using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;

namespace GoogleImageDownloader.Core.Services
{
    public class ImageDownloadService : IImageDownloadService
    {
        public void DownloadImages(IEnumerable<ImageResult> images, string targetFolder)
        {
            // TODO: Implement image download logic
            throw new System.NotImplementedException();
        }
    }
} 