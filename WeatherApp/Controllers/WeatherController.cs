﻿using Microsoft.AspNetCore.Mvc;
using WeatherApp.Logging;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService _weatherService;
        private readonly IWeatherLogger _logger;

        public WeatherController(WeatherService weatherService, IWeatherLogger logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByCity(string cityName)
        {
            _logger.Info($"Searching for city: {cityName}");

            // Get coordinates for the city
            var coordinates = await _weatherService.SearchByCityAsync(cityName);

            if (coordinates.lat.HasValue && coordinates.lon.HasValue)
            {
                // Get weather data using coordinates
                var weather = await _weatherService.GetWeatherAsync(coordinates.lat.Value, coordinates.lon.Value);
                return View("WeatherView", weather);
            }
            _logger.Error($"Error searching for city {cityName}");
            ViewBag.ErrorMessage = "City not found.";
            return View("Index");
        }
    }
}
