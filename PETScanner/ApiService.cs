namespace PETScanner
{

    using System;
    using System.Text.Json;
    using System.Text;
    using System.Threading.Tasks;
    using PETScanner.Model;



    public static class ApiService
    {
        private static readonly HttpClient httpClient = new HttpClient();


        private const string FileName = "ApiEndpoints.json";

        public static async Task<ApiEndpoints> LoadEndpointsAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(FileName);
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var endpoints = JsonSerializer.Deserialize<ApiEndpoints>(json);
            return endpoints;
        }



        // Generic POST method (no body, returns string)
        public static async Task<string> PostAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.");

            var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"POST to {url} failed with status {response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostJsonAsync(string url, object body)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.");

            var json = System.Text.Json.JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ✅ Correct overload

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"POST to {url} failed with status {response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        // Generic GET method
        public static async Task<string> GetAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.");

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"GET from {url} failed with status {response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }

}
