using GoogleImageDownloader.Core.Models;
using GoogleImageDownloader.Core.Interfaces;
using Microsoft.Win32;

namespace GoogleImageDownloader.Core.Services
{
    public class RegistryService : IRegistryService
    {
        private const string RegistryPath = @"Software\\GoogleImageDownloader\\Filters";

        #pragma warning disable CA1416
        public void SaveFilters(SearchFilters filters)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(RegistryPath))
            {
                key.SetValue(nameof(filters.Size), filters.Size ?? "");
                key.SetValue(nameof(filters.Color), filters.Color ?? "");
                key.SetValue(nameof(filters.UsageRights), filters.UsageRights ?? "");
                key.SetValue(nameof(filters.Type), filters.Type ?? "");
                key.SetValue(nameof(filters.Time), filters.Time ?? "");
                key.SetValue(nameof(filters.Query), filters.Query ?? "");
            }
        }

        public SearchFilters LoadFilters()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
            {
                if (key == null) return new SearchFilters();
                return new SearchFilters
                {
                    Size = key.GetValue(nameof(SearchFilters.Size), "") as string,
                    Color = key.GetValue(nameof(SearchFilters.Color), "") as string,
                    UsageRights = key.GetValue(nameof(SearchFilters.UsageRights), "") as string,
                    Type = key.GetValue(nameof(SearchFilters.Type), "") as string,
                    Time = key.GetValue(nameof(SearchFilters.Time), "") as string,
                    Query = key.GetValue(nameof(SearchFilters.Query), "") as string
                };
            }
        }
        #pragma warning restore CA1416
    }
} 