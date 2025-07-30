using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Downloader.Shared.WebOperations
{
    public class ScraperClass
    {
        private readonly HttpClient _httpClient;

        public ScraperClass()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/58.0.3029.110 Safari/537.3");
        }

        public async Task<string> GetPageAsStringAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<PageContent> ExtractContentAsync(string url)
        {
            var html = await GetPageAsStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var images = ExtractAttributes(doc, "//img[@src]", "src");
            var videos = ExtractAttributes(doc, "//video[@src]|//source[@src]", "src");
            var links = ExtractAttributes(doc, "//a[@href]", "href");
            var text = ExtractVisibleText(doc);

            Console.WriteLine($"Found {images.Count} images, {videos.Count} videos, {links.Count} links.");
            if (string.IsNullOrWhiteSpace(text))
                Console.WriteLine("No visible text found.");

            return new PageContent
            {
                ImageLinks = images,
                VideoLinks = videos,
                HyperLinks = links,
                TextContent = text
            };
        }

        public List<string> ExtractAttributes(HtmlDocument doc, string xpath, string attribute)
        {
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            var results = new List<string>();

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var value = node.GetAttributeValue(attribute, null)?.Trim();

                    if (!string.IsNullOrEmpty(value) && IsValidUrl(value))
                    {
                        results.Add(value);
                    }
                }
            }

            return results;
        }

        public string ExtractVisibleText(HtmlDocument doc)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var node in doc.DocumentNode.Descendants())
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    var text = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(text))
                        sb.AppendLine(text);
                }
            }
            return sb.ToString();
        }

        public bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        private bool HasAllowedExtension(string url, List<string> allowedExtensions)
        {
            try
            {
                var uri = new Uri(url);
                var ext = System.IO.Path.GetExtension(uri.AbsolutePath)?.ToLower();
                return allowedExtensions.Contains(ext);
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> IsUrlAccessibleAsync(string url)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Head, url);
                using var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<string>> ExtractFilteredAttributesAsync(HtmlDocument doc, string xpath, string attribute, List<string> allowedExtensions = null)
        {
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            var results = new HashSet<string>(); // automatycznie usuwa duplikaty

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var value = node.GetAttributeValue(attribute, null)?.Trim();

                    if (!string.IsNullOrEmpty(value) && IsValidUrl(value))
                    {
                        if (allowedExtensions == null || HasAllowedExtension(value, allowedExtensions))
                        {
                            if (await IsUrlAccessibleAsync(value))
                            {
                                results.Add(value);
                            }
                        }
                    }
                }
            }

            return results.ToList();
        }
        private bool IsFromDomain(string url, string domain)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Host.EndsWith(domain, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}