using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader.Shared.WebOperations
{
    public class DownloderClass
    {
        public DownloderClass()
        {
            // Initialize any necessary components or services here
        }
        public async Task DownloadImagesAsync(string imageUrl, string savePath)
        {
            using HttpClient client = new HttpClient();
            try
            {
                var imageBytes = await client.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(savePath, imageBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image: {ex.Message}");
                return;
            }
        }
        public async Task DownloadVideoAsync(string videoUrl, string savePath)
        {
            using HttpClient client = new HttpClient();
            try
            {
                var videoBytes = await client.GetByteArrayAsync(videoUrl);
                await File.WriteAllBytesAsync(savePath, videoBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image: {ex.Message}");
                return;
            }
        }
        public async Task<bool> ResourceExistsAsync(string url)
        {
            using var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> GetContentTypeAsync(string url)
        {
            using var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await client.SendAsync(request);
                return response.Content.Headers.ContentType?.MediaType ?? "";
            }
            catch
            {
                return "";
            }
        }
        public async Task DownloadResourceAsync(string url, string savePath)
        {
            var contentType = await GetContentTypeAsync(url);

            if (string.IsNullOrEmpty(contentType))
            {
                Console.WriteLine("Nie udało się pobrać typu zasobu.");
                return;
            }

            if (contentType.StartsWith("image/"))
            {
                await DownloadImagesAsync(url, savePath);
            }
            else if (contentType.StartsWith("video/"))
            {
                await DownloadVideoAsync(url, savePath);
            }
            else if (contentType.StartsWith("text/"))
            {
                using var client = new HttpClient();
                var text = await client.GetStringAsync(url);
                await File.WriteAllTextAsync(savePath, text);
            }
            else
            {
                Console.WriteLine($"Nieobsługiwany typ: {contentType}");
            }
        }
    }
}
