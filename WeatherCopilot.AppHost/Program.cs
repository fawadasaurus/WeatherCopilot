var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WeatherCopilot_ApiService>("apiservice")
    .WithExternalHttpEndpoints();

builder.AddNpmApp("react", "../WeatherCopilot.React")
    .WithReference(apiService)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();