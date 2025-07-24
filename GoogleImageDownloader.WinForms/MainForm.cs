using System;
using System.Windows.Forms;
using GoogleImageDownloader.Core.Interfaces;
using GoogleImageDownloader.Core.Models;
using System.Collections.Generic;
using System.IO;

namespace GoogleImageDownloader.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IGoogleImageSearchService _searchService;
        private readonly IImageDownloadService _downloadService;
        private readonly IRegistryService _registryService;

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFiltersToRegistry();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnDownload.Enabled = false;
            lvImages.Items.Clear();
            imageListLarge.Images.Clear();
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
                var results = _searchService.SearchImages(filters);
                int idx = 0;
                foreach (var img in results)
                {
                    System.Drawing.Image thumb = null;
                    try
                    {
                        using (var wc = new System.Net.WebClient())
                        {
                            using (var stream = wc.OpenRead(img.ThumbnailUrl))
                            {
                                thumb = System.Drawing.Image.FromStream(stream);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Optionally log ex
                        thumb = null;
                    }
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
                    idx++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during search:\n{ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
                btnDownload.Enabled = true;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnDownload.Enabled = false;
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
                    return;
                }
                var searchTerm = txtSearch.Text.Trim();
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                var targetFolder = Path.Combine(downloads, $"{searchTerm}_{timestamp}");
                _downloadService.DownloadImages(selectedImages, targetFolder);
                MessageBox.Show($"Downloaded {selectedImages.Count} images to:\n{targetFolder}", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during download:\n{ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
                btnDownload.Enabled = true;
            }
        }
    }
}
