using SearchImageDownloader.Core.Models;
using SearchImageDownloader.Core.Interfaces;
using Microsoft.Win32;

namespace SearchImageDownloader.Core.Services
{
    public class RegistryService : IRegistryService
    {
        private const string RegistryPath = @"Software\\SearchImageDownloader\\Filters";

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

        public void SaveGoogleApiCredentials(string apiKey, string cseId)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(RegistryPath))
            {
                key.SetValue("GoogleApiKey", apiKey ?? "");
                key.SetValue("GoogleCseId", cseId ?? "");
            }
        }

        public (string ApiKey, string CseId) LoadGoogleApiCredentials()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
            {
                if (key == null) return ("", "");
                var apiKey = key.GetValue("GoogleApiKey", "") as string ?? "";
                var cseId = key.GetValue("GoogleCseId", "") as string ?? "";
                return (apiKey, cseId);
            }
        }

        public static void GenerateRegFile(string apiKey, string cseId, string outputPath)
        {
            var regContent =
                "Windows Registry Editor Version 5.00\r\n\r\n" +
                "[HKEY_CURRENT_USER\\Software\\SearchImageDownloader\\Filters]\r\n" +
                $"\"GoogleApiKey\"=\"{apiKey}\"\r\n" +
                $"\"GoogleCseId\"=\"{cseId}\"\r\n";
            System.IO.File.WriteAllText(outputPath, regContent);
        }
        #pragma warning restore CA1416
    }

#if DEBUG
    public static class RegFileGenerator
    {
        public static void Main(string[] args)
        {
            // Usage: dotnet run --project SearchImageDownloader.Core --no-build <apikey> <cseid> <output.reg>
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: dotnet run --project SearchImageDownloader.Core --no-build <apikey> <cseid> <output.reg>");
                return;
            }
            RegistryService.GenerateRegFile(args[0], args[1], args[2]);
            Console.WriteLine($"Registry file generated at {args[2]}");
        }
    }
#endif
} 