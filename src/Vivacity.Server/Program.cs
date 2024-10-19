using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Vivacity.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet
(
    "/configurations/{configurationName}",
    (
        [FromServices] IOptionsSnapshot<Vivacity.Server.Options.ConfigurationsOptions> configurationsOptions,
        [FromServices] ConfigurationLoader configurationLoader,
        [FromRoute] string configurationName
    ) =>
    {
        var configuration = configurationsOptions.Value.GetConfigurationByName(configurationName);
        if (configuration is null)
            return Results.NotFound();
        
        var configurationJson = configurationLoader.Load(configuration.Uri);
        return Results.Ok(configurationJson);
    });

app.Run();