using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WeatherApp.Helpers
{
    public class ConfigHelper
    {
   
        private readonly string _apiKey;
        private readonly string _modelname;

        public ConfigHelper(IConfiguration configuration)
        {
            _apiKey = configuration["HuggingFace:ApiKey"];
            _modelname = configuration["HuggingFace:ModelName"];
        }

        public string GetApiKey()
        {
            return _apiKey;
        }

        // Method to retrieve Hugging Face Model Name
        public string GetModelName()
        {
            return _modelname;
        }
    }
}
