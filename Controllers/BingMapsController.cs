using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BingMapsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey =  Environment.GetEnvironmentVariable("BingMapsApiKey");

        public BingMapsController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("restaurants")]

        public async Task<IActionResult> GetRestaurants(double latitude, double longitude, int radius)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                
                var url = $"http://spatial.virtualearth.net/REST/v1/data/Microsoft/PointsOfInterest?spatialFilter=nearby({latitude},{longitude},{radius})&$filter=EntityTypeID%20eq%20'Restaurant'&key={_apiKey}";


                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve restaurants.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("place/{entityId}")]
        public async Task<IActionResult> GetPlaceDetails(string entityId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"http://spatial.virtualearth.net/REST/v1/data/2191359/dataSourceName/entityTypeName({entityId})?jsonp=jsonCallBackFunction&jsonso=jsonState&isStaging=isStaging&key={_apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve place details.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


    }
}
