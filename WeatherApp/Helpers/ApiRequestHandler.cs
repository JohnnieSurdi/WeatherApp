namespace WeatherApp.Helpers
{
    public class ApiRequestHandler
    {
        private readonly HttpClient _httpClient;

        public ApiRequestHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ExecuteApiCallAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Network error occurred while making the API request.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during API call: " + ex.Message);
            }
        }
    }
}
