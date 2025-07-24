using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;

namespace GoogleImageDownloader.Core.Interfaces
{
    public interface IImageDownloadService
    {
        void DownloadImages(IEnumerable<ImageResult> images, string targetFolder);
    }
} 