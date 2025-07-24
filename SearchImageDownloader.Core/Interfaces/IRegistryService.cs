using SearchImageDownloader.Core.Models;

namespace SearchImageDownloader.Core.Interfaces
{
    public interface IRegistryService
    {
        void SaveFilters(SearchFilters filters);
        SearchFilters LoadFilters();
        void SaveGoogleApiCredentials(string apiKey, string cseId);
        (string ApiKey, string CseId) LoadGoogleApiCredentials();
    }
} 