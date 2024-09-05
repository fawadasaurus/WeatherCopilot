using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public struct WeatherForecast
{
    [JsonPropertyName("@context")]
    public List<object> Context { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("geometry")]
    public ForecastGeometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public WeatherProperties Properties { get; set; }
}

public struct ForecastGeometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public List<List<List<double>>> Coordinates { get; set; }
}

public struct WeatherProperties
{
    [JsonPropertyName("units")]
    public string Units { get; set; }

    [JsonPropertyName("forecastGenerator")]
    public string ForecastGenerator { get; set; }

    [JsonPropertyName("generatedAt")]
    public DateTimeOffset GeneratedAt { get; set; }

    [JsonPropertyName("updateTime")]
    public DateTimeOffset UpdateTime { get; set; }

    [JsonPropertyName("validTimes")]
    public string ValidTimes { get; set; }

    [JsonPropertyName("elevation")]
    public WeatherQuantitativeValue Elevation { get; set; }

    [JsonPropertyName("periods")]
    public List<ForecastPeriod> Periods { get; set; }
}

public struct WeatherQuantitativeValue
{
    [JsonPropertyName("unitCode")]
    public string UnitCode { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }
}

public struct ForecastPeriod
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("startTime")]
    public DateTimeOffset StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [JsonPropertyName("isDaytime")]
    public bool IsDaytime { get; set; }

    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }

    [JsonPropertyName("temperatureUnit")]
    public string TemperatureUnit { get; set; }

    [JsonPropertyName("temperatureTrend")]
    public string TemperatureTrend { get; set; }

    [JsonPropertyName("probabilityOfPrecipitation")]
    public object ProbabilityOfPrecipitation { get; set; }

    [JsonPropertyName("windSpeed")]
    public string WindSpeed { get; set; }

    [JsonPropertyName("windDirection")]
    public string WindDirection { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("shortForecast")]
    public string ShortForecast { get; set; }

    [JsonPropertyName("detailedForecast")]
    public string DetailedForecast { get; set; }
}
