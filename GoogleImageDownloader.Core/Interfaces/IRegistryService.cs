using GoogleImageDownloader.Core.Models;

namespace GoogleImageDownloader.Core.Interfaces
{
    public interface IRegistryService
    {
        void SaveFilters(SearchFilters filters);
        SearchFilters LoadFilters();
    }
} 