public class BingMapsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public BingMapsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["BingMaps:ApiKey"];
    }

    public async Task<IEnumerable<Place>> GetNearbyRestaurants(double latitude, double longitude)
    {
        // Make a request to Bing Maps API to search for nearby restaurants
        // Example endpoint: https://dev.virtualearth.net/REST/v1/Search?key=API_KEY&query=restaurants&lat=latitude&lon=longitude
        var response = await _httpClient.GetAsync($"https://dev.virtualearth.net/REST/v1/Search?key={_apiKey}&query=restaurants&lat={latitude}&lon={longitude}");

        // Parse the response and extract relevant data
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            // Process the response and return a list of nearby restaurants
            // Example: deserialize JSON response and extract restaurant details
        }
        else
        {
            // Handle error response
            // Example: log error message, return empty list, or throw exception
        }
    }

    public async Task<PlaceDetails> GetPlaceDetails(string placeId)
    {
        // Make a request to Bing Maps API to get detailed information about a place
        // Example endpoint: https://dev.virtualearth.net/REST/v1/Locations/placeId?key=API_KEY
        var response = await _httpClient.GetAsync($"https://dev.virtualearth.net/REST/v1/Locations/{placeId}?key={_apiKey}");

        // Parse the response and extract relevant data
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            // Process the response and return detailed place information
            // Example: deserialize JSON response and extract place details
        }
        else
        {
            // Handle error response
            // Example: log error message, return null, or throw exception
        }
    }
}
