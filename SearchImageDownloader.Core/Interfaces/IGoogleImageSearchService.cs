using System.Collections.Generic;
using SearchImageDownloader.Core.Models;

namespace SearchImageDownloader.Core.Interfaces
{
    public interface IImageSearchService
    {
        IEnumerable<SearchImageDownloader.Core.Models.ImageResult> SearchImages(SearchImageDownloader.Core.Models.SearchFilters filters);
    }
} 