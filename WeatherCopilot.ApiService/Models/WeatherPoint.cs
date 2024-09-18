// Models/WeatherPoint.cs

using System.Text.Json.Serialization;

public struct WeatherPoint
{
    [JsonPropertyName("@context")]
    public object[] Context { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }
}

public struct Geometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; set; }
}

public struct Properties
{
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    [JsonPropertyName("cwa")]
    public string Cwa { get; set; }

    [JsonPropertyName("forecastOffice")]
    public string ForecastOffice { get; set; }

    [JsonPropertyName("gridId")]
    public string GridId { get; set; }

    [JsonPropertyName("gridX")]
    public int GridX { get; set; }

    [JsonPropertyName("gridY")]
    public int GridY { get; set; }

    [JsonPropertyName("forecast")]
    public string Forecast { get; set; }

    [JsonPropertyName("forecastHourly")]
    public string ForecastHourly { get; set; }

    [JsonPropertyName("forecastGridData")]
    public string ForecastGridData { get; set; }

    [JsonPropertyName("observationStations")]
    public string ObservationStations { get; set; }

    [JsonPropertyName("relativeLocation")]
    public RelativeLocation RelativeLocation { get; set; }

    [JsonPropertyName("forecastZone")]
    public string ForecastZone { get; set; }

    [JsonPropertyName("county")]
    public string County { get; set; }

    [JsonPropertyName("fireWeatherZone")]
    public string FireWeatherZone { get; set; }

    [JsonPropertyName("timeZone")]
    public string TimeZone { get; set; }

    [JsonPropertyName("radarStation")]
    public string RadarStation { get; set; }
}

public struct RelativeLocation
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public RelativeLocationProperties Properties { get; set; }
}

public struct RelativeLocationProperties
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("distance")]
    public object Distance { get; set; }

    [JsonPropertyName("bearing")]
    public object Bearing { get; set; }
}
