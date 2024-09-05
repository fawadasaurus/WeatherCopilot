using Azure.Identity;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using WeatherCopilot.Controllers;

var azureAIUrl = builder.Configuration["AzureAIUrl"];
var deploymentName = builder.Configuration["AzureAIDeploymentName"];

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<WebForecast>();
builder.Services.AddHttpClient<LocationService>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "WeatherCopilot");
});
builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "WeatherCopilot");
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapGet("/weatherforecast", ([FromServices] WebForecast forecastService, string city, string state) => forecastService.GetForecastsAsync(city, state))
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
