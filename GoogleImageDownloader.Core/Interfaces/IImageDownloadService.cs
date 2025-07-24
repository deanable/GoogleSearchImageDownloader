using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleImageDownloader.Core.Models;

namespace GoogleImageDownloader.Core.Interfaces
{
    public interface IImageDownloadService
    {
        Task DownloadImagesAsync(IEnumerable<ImageResult> images, string targetFolder);
    }
} 