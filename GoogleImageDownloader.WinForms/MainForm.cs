using System;
using System.Windows.Forms;
using GoogleImageDownloader.Core.Interfaces;
using GoogleImageDownloader.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleImageDownloader.Core.Services;

namespace GoogleImageDownloader.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IGoogleImageSearchService _searchService;
        private readonly IImageDownloadService _downloadService;
        private readonly IRegistryService _registryService;

        private const int PageSize = 30;
        private List<GoogleImageDownloader.Core.Models.ImageResult> allResults = new();
        private int currentPage = 0;
        private Button btnLoadMore;
        private readonly string logFilePath;

        public MainForm(IGoogleImageSearchService searchService, IImageDownloadService downloadService, IRegistryService registryService)
        {
            _searchService = searchService;
            _downloadService = downloadService;
            _registryService = registryService;
            InitializeComponent();
            LoadFiltersFromRegistry();
            btnSearch.Click += btnSearch_Click;
            btnDownload.Click += btnDownload_Click;
            cmbSize.SelectedIndexChanged += FilterChanged;
            cmbColor.SelectedIndexChanged += FilterChanged;
            cmbUsageRights.SelectedIndexChanged += FilterChanged;
            cmbType.SelectedIndexChanged += FilterChanged;
            cmbTime.SelectedIndexChanged += FilterChanged;
            this.FormClosing += Form1_FormClosing;
            InitializeLazyLoading();
            // Setup log file
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
            logFilePath = Path.Combine(logDir, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            Log($"Log started for session at {DateTime.Now}");
            CoreLogger.Init(logFilePath);
        }

        private void LoadFiltersFromRegistry()
        {
            var filters = _registryService.LoadFilters();
            txtSearch.Text = filters.Query ?? string.Empty;
            cmbSize.SelectedItem = string.IsNullOrEmpty(filters.Size) ? "Any size" : filters.Size;
            cmbColor.SelectedItem = string.IsNullOrEmpty(filters.Color) ? "Any color" : filters.Color;
            cmbUsageRights.SelectedItem = string.IsNullOrEmpty(filters.UsageRights) ? "Any rights" : filters.UsageRights;
            cmbType.SelectedItem = string.IsNullOrEmpty(filters.Type) ? "Any type" : filters.Type;
            cmbTime.SelectedItem = string.IsNullOrEmpty(filters.Time) ? "Any time" : filters.Time;
        }

        private void SaveFiltersToRegistry()
        {
            var filters = new SearchFilters
            {
                Query = txtSearch.Text,
                Size = cmbSize.SelectedItem?.ToString() ?? "Any size",
                Color = cmbColor.SelectedItem?.ToString() ?? "Any color",
                UsageRights = cmbUsageRights.SelectedItem?.ToString() ?? "Any rights",
                Type = cmbType.SelectedItem?.ToString() ?? "Any type",
                Time = cmbTime.SelectedItem?.ToString() ?? "Any time"
            };
            _registryService.SaveFilters(filters);
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            SaveFiltersToRegistry();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveFiltersToRegistry();
            Log($"Log ended for session at {DateTime.Now}");
            CoreLogger.Log($"Log ended for session at {DateTime.Now}");
        }

        private void InitializeLazyLoading()
        {
            btnLoadMore = new Button();
            btnLoadMore.Text = "Load More";
            btnLoadMore.Size = new System.Drawing.Size(120, 30);
            btnLoadMore.Location = new System.Drawing.Point(140, 440);
            btnLoadMore.Click += BtnLoadMore_Click;
            this.Controls.Add(btnLoadMore);
            btnLoadMore.Visible = false;
        }

        private void Log(string message)
        {
            var logLine = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";
            File.AppendAllText(logFilePath, logLine);
        }

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnDownload.Enabled = false;
            lvImages.Items.Clear();
            imageListLarge.Images.Clear();
            progressBar.Value = 0;
            lblStatus.Text = "Searching...";
            Log($"Search started: '{txtSearch.Text}'");
            try
            {
                var filters = new SearchFilters
                {
                    Query = txtSearch.Text,
                    Size = cmbSize.SelectedItem?.ToString() ?? "Any size",
                    Color = cmbColor.SelectedItem?.ToString() ?? "Any color",
                    UsageRights = cmbUsageRights.SelectedItem?.ToString() ?? "Any rights",
                    Type = cmbType.SelectedItem?.ToString() ?? "Any type",
                    Time = cmbTime.SelectedItem?.ToString() ?? "Any time"
                };
                await Task.Run(() =>
                {
                    allResults = _searchService.SearchImages(filters).ToList();
                });
                currentPage = 0;
                progressBar.Maximum = Math.Min(PageSize, allResults.Count);
                progressBar.Value = 0;
                lblStatus.Text = $"Found {allResults.Count} images. Loading first {Math.Min(PageSize, allResults.Count)}...";
                Log($"Found {allResults.Count} images.");
                await Task.Run(() => LoadNextPageThreaded());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during search:\n{ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error during search.";
                Log($"Search error: {ex.Message}");
            }
            finally
            {
                btnSearch.Enabled = true;
                btnDownload.Enabled = true;
            }
        }

        private void LoadNextPageThreaded()
        {
            int start = currentPage * PageSize;
            int end = Math.Min(start + PageSize, allResults.Count);
            Invoke(new Action(() =>
            {
                progressBar.Value = 0;
                progressBar.Maximum = end - start;
                lblStatus.Text = $"Loading images {start + 1}-{end} of {allResults.Count}";
            }));
            using (var http = new System.Net.Http.HttpClient())
            {
                for (int i = start; i < end; i++)
                {
                    var img = allResults[i];
                    System.Drawing.Image? thumb = null;
                    try
                    {
                        if (!string.IsNullOrEmpty(img.ThumbnailUrl))
                        {
                            var data = http.GetByteArrayAsync(img.ThumbnailUrl).Result;
                            using (var ms = new System.IO.MemoryStream(data))
                            {
                                thumb = System.Drawing.Image.FromStream(ms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Thumbnail load error for image {i + 1}: {ex.Message}");
                        thumb = null;
                    }
                    int idx = i;
                    Invoke(new Action(() =>
                    {
                        if (thumb != null)
                        {
                            imageListLarge.Images.Add(idx.ToString(), thumb);
                        }
                        var item = new ListViewItem(img.Title ?? "Image")
                        {
                            ImageIndex = thumb != null ? idx : -1,
                            Tag = img
                        };
                        lvImages.Items.Add(item);
                        progressBar.Value = idx - start + 1;
                    }));
                }
            }
            currentPage++;
            Invoke(new Action(() =>
            {
                btnLoadMore.Visible = currentPage * PageSize < allResults.Count;
                lblStatus.Text = btnLoadMore.Visible ? $"Loaded {end} of {allResults.Count}. Click 'Load More' for more." : "All images loaded.";
                Log($"Loaded images {start + 1}-{end}.");
            }));
        }

        private void BtnLoadMore_Click(object? sender, EventArgs e)
        {
            Task.Run(() => LoadNextPageThreaded());
        }

        private async void btnDownload_Click(object? sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnDownload.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "Downloading...";
            Log("Download started.");
            try
            {
                var selectedImages = new List<GoogleImageDownloader.Core.Models.ImageResult>();
                foreach (ListViewItem item in lvImages.CheckedItems)
                {
                    if (item.Tag is GoogleImageDownloader.Core.Models.ImageResult img)
                        selectedImages.Add(img);
                }
                if (selectedImages.Count == 0)
                {
                    MessageBox.Show("Please select images to download.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "No images selected.";
                    Log("No images selected for download.");
                    return;
                }
                var searchTerm = txtSearch.Text.Trim();
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                var targetFolder = Path.Combine(downloads, $"{searchTerm}_{timestamp}");
                progressBar.Maximum = selectedImages.Count;
                progressBar.Value = 0;
                int done = 0;
                foreach (var img in selectedImages)
                {
                    await Task.Run(() => _downloadService.DownloadImagesAsync(new[] { img }, targetFolder).Wait());
                    done++;
                    progressBar.Value = done;
                    lblStatus.Text = $"Downloading {done} of {selectedImages.Count}...";
                    Log($"Downloaded {done}/{selectedImages.Count}: {img.ImageUrl}");
                }
                MessageBox.Show($"Downloaded {selectedImages.Count} images to:\n{targetFolder}", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Download complete.";
                Log("Download complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during download:\n{ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error during download.";
                Log($"Download error: {ex.Message}");
            }
            finally
            {
                btnSearch.Enabled = true;
                btnDownload.Enabled = true;
            }
        }
    }
}
