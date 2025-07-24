using System;
using System.Windows.Forms;
using GoogleImageDownloader.Core.Interfaces;
using GoogleImageDownloader.Core.Models;

namespace GoogleImageDownloader.WinForms
{
    public partial class Form1 : Form
    {
        private readonly IGoogleImageSearchService _searchService;
        private readonly IImageDownloadService _downloadService;
        private readonly IRegistryService _registryService;

        public Form1(IGoogleImageSearchService searchService, IImageDownloadService downloadService, IRegistryService registryService)
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
            lvImages.Items.Clear();
            imageListLarge.Images.Clear();
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
                // Download thumbnail (synchronously for now)
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
                catch { thumb = null; }
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
                // TODO: Implement lazy loading for large result sets
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // TODO: Get selected images from lvImages and call _downloadService.DownloadImages
        }
    }
}
