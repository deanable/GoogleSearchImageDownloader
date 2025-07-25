using System.IO;
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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Advanced logging setup
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            var logFile = Path.Combine(logDir, $"app-{DateTime.Now:yyyyMMdd}.log");
            CoreLogger.Init(logFile);
            CoreLogger.Log($"Application started at {DateTime.Now}");
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                CoreLogger.Log($"Unhandled exception: {e.ExceptionObject}");
            };

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