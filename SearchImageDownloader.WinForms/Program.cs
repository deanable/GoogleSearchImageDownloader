using SearchImageDownloader.Core.Services;
using SearchImageDownloader.Core.Interfaces;

namespace SearchImageDownloader.WinForms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            IRegistryService registryService = new RegistryService();
            IImageSearchService searchService = new GoogleImageSearchService(registryService);
            IImageDownloadService downloadService = new ImageDownloadService();
            Application.Run(new MainForm(searchService, downloadService, registryService));
        }
    }
}