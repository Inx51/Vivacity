using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Vivacity.Server.Extensions;
using Vivacity.Server.Services;
using ConfigurationsOptions = Vivacity.Server.Options.ConfigurationsOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConfigurationsOptions>(builder.Configuration.GetSection(ConfigurationsOptions.SectionName));

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ConfigurationLoader>();
builder.Services.AddSingleton<FileSystem>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet
(
    "/configurations/{configurationName}",
    (
        [FromServices] IOptionsSnapshot<ConfigurationsOptions> configurationsOptions,
        [FromServices] ConfigurationLoader configurationLoader,
        [FromRoute] string configurationName
    ) =>
    {
        var configuration = configurationsOptions.Value.GetConfigurationByName(configurationName);
        if (configuration is null)
            return Results.NotFound();
        
        var configurationJson = configurationLoader.Load(configuration.Name, configuration.Uri);
        return Results.Ok(configurationJson);
    });

app.Run();