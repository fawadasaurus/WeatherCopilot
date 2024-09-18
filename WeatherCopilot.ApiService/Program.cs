// program.cs

using Azure.Identity;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using WeatherCopilot.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.AddServiceDefaults();

//Register Azure AI Chat Client
var azureAIUrl = builder.Configuration["AzureAIUrl"] ?? "Not Found";
var deploymentName = builder.Configuration["AzureAIDeploymentName"] ?? "Not Found";
var credential = new DefaultAzureCredential();
var azureClient = new AzureOpenAIClient(new Uri(azureAIUrl), credential);
var chatClient = azureClient.GetChatClient(deploymentName);

builder.Services.AddSingleton(chatClient);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddScoped<WebForecast>();
builder.Services.AddScoped<ForecastService>();
builder.Services.AddScoped<AIService>();
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

app.MapPost("/weatherforecastchat", ([FromServices] WebForecast forecastService, [FromBody] WebForecast.ChatMessage message, bool useTool) => forecastService.GetChatResponseAsync(message, useTool))
    .WithName("Chat")
    .WithOpenApi();

app.Run();
