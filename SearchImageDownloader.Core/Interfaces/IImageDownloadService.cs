using System.Collections.Generic;
using System.Threading.Tasks;
using SearchImageDownloader.Core.Models;

namespace SearchImageDownloader.Core.Interfaces
{
    public interface IImageDownloadService
    {
        Task DownloadImagesAsync(IEnumerable<ImageResult> images, string targetFolder);
    }
} 