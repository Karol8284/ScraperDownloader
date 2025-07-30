using System;
using System.Collections.Generic;

namespace Downloader.Shared.WebOperations
{
    public class PageContent
    {
        public List<string> ImageLinks { get; set; }
        public List<string> VideoLinks { get; set; }
        public List<string> HyperLinks { get; set; }
        public string TextContent { get; set; }

        public PageContent()
        {
            ImageLinks = new List<string>();
            VideoLinks = new List<string>();
            HyperLinks = new List<string>();
            TextContent = string.Empty;
        }

        public void GetCounts()
        {
            Console.WriteLine($"🖼️ Obrazy: {ImageLinks.Count}");
            Console.WriteLine($"🎬 Filmy: {VideoLinks.Count}");
            Console.WriteLine($"🔗 Linki: {HyperLinks.Count}");
            Console.WriteLine($"📝 Długość tekstu: {TextContent.Length} znaków");
        }
        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}