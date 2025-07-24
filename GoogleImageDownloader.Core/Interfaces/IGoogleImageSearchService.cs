using System.Collections.Generic;
using GoogleImageDownloader.Core.Models;

namespace GoogleImageDownloader.Core.Interfaces
{
    public interface IGoogleImageSearchService
    {
        IEnumerable<ImageResult> SearchImages(SearchFilters filters);
    }
} 