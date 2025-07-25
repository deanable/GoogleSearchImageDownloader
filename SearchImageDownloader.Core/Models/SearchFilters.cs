namespace SearchImageDownloader.Core.Models
{
    public class SearchFilters
    {
        public string? Size { get; set; } // e.g., "large", "medium", "icon"
        public string? Color { get; set; } // e.g., "color", "blackandwhite", "transparent"
        public string? UsageRights { get; set; } // e.g., "labeled-for-reuse"
        public string? Type { get; set; } // e.g., "photo", "clipart", "lineart"
        public string? Time { get; set; } // e.g., "past-24-hours", "past-week"
        public string? Query { get; set; } // The search term
        public int Start { get; set; } = 1; // Google API: 1-based index
        public int Num { get; set; } = 10; // Google API: max 10 per request
    }
} 