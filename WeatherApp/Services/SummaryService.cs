
using Microsoft.SemanticKernel;

namespace WeatherApp.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly Kernel _kernel;

        public SummaryService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> GenerateSummaryAsync(string cityName)
        {
            try
            {
                return await _kernel.InvokePromptAsync<string>($"Tell me something about {cityName}");
            }
            catch (Exception ex)
            {
                return $"Nothing to say about {cityName}"; 
            }
        }
    }
}
