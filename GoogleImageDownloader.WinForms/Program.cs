using GoogleImageDownloader.Core.Services;
using GoogleImageDownloader.Core.Interfaces;

namespace GoogleImageDownloader.WinForms
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
            IGoogleImageSearchService searchService = new GoogleImageSearchService();
            IImageDownloadService downloadService = new ImageDownloadService();
            IRegistryService registryService = new RegistryService();
            Application.Run(new Form1(searchService, downloadService, registryService));
        }
    }
}