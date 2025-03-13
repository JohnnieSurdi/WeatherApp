using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IWeatherSearchRepository _weatherSearchRepository;
        private readonly WeatherApp.Logging.ILogger _logger;
        private readonly ISummaryService _summaryService;

        public WeatherController(IWeatherService weatherService, IWeatherSearchRepository weatherSearchRepository, WeatherApp.Logging.ILogger logger, ISummaryService summaryService)
        {
            _weatherService = weatherService;
            _weatherSearchRepository = weatherSearchRepository;
            _logger = logger;
            _summaryService = summaryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RecentSearches()
        {
            var allSearches = await _weatherSearchRepository.GetRecentWeatherSearchesAsync();
            return View(allSearches);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByCity(string cityName)
        {
            _logger.Info($"Searching for city: {cityName}");

            var coordinates = await _weatherService.SearchByCityAsync(cityName);

            if (coordinates.lat.HasValue && coordinates.lon.HasValue)
            {
                var weather = await _weatherService.GetWeatherAsync(coordinates.lat.Value, coordinates.lon.Value);
                var summary = await _summaryService.GenerateSummaryAsync(weather.Location.CityName);
                ViewBag.Summary = summary;
                return View("WeatherView", weather);
            }
            _logger.Error($"Error searching for city {cityName}");
            ViewBag.ErrorMessage = "City not found.";
            return View("Index");
        }
    }
}
