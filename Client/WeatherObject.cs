using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Weather
{
    private string apiKey = "0a7f47c86a31cd32f1d61efabed64b98";
    public WeatherForecast? weatherForecast;

    public Weather(){}
    /*public Weather(float lat, float lon)
    {
        weatherForecast = GetWeatherObjectFromCoords(lat, lon, apiKey);
    }
    public Weather(string locationName)
    {
        Location location = GetLocationFromName(locationName, apiKey);
        weatherForecast = GetWeatherObjectFromLocation(location, apiKey);
    }*/

    public async Task MakeForecastFromLocationAsync(string locationName)
    {
        Location location = await GetLocationFromNameAsync(locationName, apiKey);
        weatherForecast = await GetWeatherObjectFromLocationAsync(location, apiKey);
    }

    public async Task MakeForecastFromCoords(float lat, float lon)
    {
        weatherForecast = await GetWeatherObjectFromCoordsAsync(lat, lon, apiKey);
    }

    /*public string GetJsonFromUrl(string url)
    {
        HttpClient client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = client.Send(request);
        using var reader = new StreamReader(response.Content.ReadAsStream());
        var jsonText = reader.ReadToEnd();

        return jsonText;
    }*/
    public async Task<string> GetJsonFromUrlAsync(string url)
    {
        try
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);
            //string jsonText = await response.Content.ReadAsStringAsync();
            return response;
        }
        catch (HttpRequestException e)
        {
            return e.ToString();
        }
    }

    /*public Location GetLocationFromName(string location, string apiKey)
    {
        string locationJson = GetJsonFromUrl($"http://api.openweathermap.org/geo/1.0/direct?q={location}&appid={apiKey}");
        Location[]? locations = JsonSerializer.Deserialize<Location[]>(locationJson);
        return locations[0];
    }*/
    public async Task<Location> GetLocationFromNameAsync(string location, string apiKey)
    {
        string locationJson = await GetJsonFromUrlAsync($"https://api.openweathermap.org/geo/1.0/direct?q={location}&appid={apiKey}");
        Location[]? locations = JsonSerializer.Deserialize<Location[]>(locationJson);
        return locations[0];
    }

    /*public WeatherForecast GetWeatherObjectFromCoords(float lat, float lon, string apiKey)
    {
        string weatherJson = GetJsonFromUrl($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&appid={apiKey}");
        WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherJson);
        return weatherForecast;
    }*/
    public async Task<WeatherForecast> GetWeatherObjectFromCoordsAsync(float lat, float lon, string apiKey)
    {
        string weatherJson = await GetJsonFromUrlAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&appid={apiKey}");
        WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherJson);
        return weatherForecast;
    }

    /*public WeatherForecast GetWeatherObjectFromLocation(Location location, string apiKey)
    {
        string weatherJson = GetJsonFromUrl($"https://api.openweathermap.org/data/2.5/onecall?lat={location.lat}&lon={location.lon}&appid={apiKey}");
        WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherJson);
        weatherForecast.location = location;
        return weatherForecast;
    }*/
    public async Task<WeatherForecast> GetWeatherObjectFromLocationAsync(Location location, string apiKey)
    {
        string weatherJson = await GetJsonFromUrlAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={location.lat}&lon={location.lon}&appid={apiKey}");
        WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherJson);
        weatherForecast.location = location;
        return weatherForecast;
    }
}

public class WeatherForecast
{
    public float lat { get; set; }
    public float lon { get; set; }
    public int timezone_offset { get; set; }

    public Location? location;
    public WeatherCurrent? current { get; set; }
}

public class Location
{
    public string? name { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public string? country { get; set; }
    public string? state { get; set; }
}

public class WeatherCurrent
{
    public int dt { get; set; }
    public int sunrise { get; set; }
    public int sunset { get; set; }
    public float temp { get; set; }
    public float feels_like { get; set; }
}