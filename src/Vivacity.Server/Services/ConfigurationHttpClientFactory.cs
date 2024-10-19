using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Vivacity.Server.Factories;
using Vivacity.Server.Options;
using Vivacity.Server.Extensions;
using ConfigurationsOptions = Vivacity.Server.Options.ConfigurationsOptions;

namespace Vivacity.Server.Services;

public class ConfigurationHttpClientFactory
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConfigurationHttpMessageHandlerFactory _configurationHttpMessageHandlerFactory;
    private readonly IOptionsSnapshot<Options.ConfigurationsOptions> _configurations;

    public ConfigurationHttpClientFactory
    (
        IMemoryCache memoryCache,
        ConfigurationHttpMessageHandlerFactory configurationHttpMessageHandlerFactory,
        IOptionsSnapshot<Options.ConfigurationsOptions> configurations
    )
    {
        _memoryCache = memoryCache;
        _configurationHttpMessageHandlerFactory = configurationHttpMessageHandlerFactory;
        _configurations = configurations;
    }
    
    public HttpClient? Get(string configurationName)
    {
        if (_memoryCache.TryGetValue(configurationName, out HttpClient? client))
            return client;

        return null;
    }
    
    public HttpClient Create(string configurationName)
    {
        var httpClient = CreateClient(configurationName);
        CacheClient(configurationName, httpClient);
        return httpClient;
    }

    private void CacheClient(string configurationName, HttpClient httpClient)
    {
        var entry = _memoryCache.CreateEntry(configurationName);
        entry.Value = httpClient;
        entry.SlidingExpiration = TimeSpan.FromMinutes(16);
        entry.RegisterPostEvictionCallback(DisposeHttpClientIfNotUsed);
    }

    private HttpClient CreateClient(string configurationName)
    {
        var handler = _configurationHttpMessageHandlerFactory.Create(configurationName);
        var httpClient = new HttpClient(handler);
        ConfigureHttpClient(configurationName, httpClient);
        return httpClient;
    }

    private void ConfigureHttpClient(string configurationName, HttpClient httpClient)
    {
        var configuration = _configurations.Value.GetConfigurationByName(configurationName);

        AppendDefaultHeaders(httpClient, configuration!);

        throw new NotImplementedException();
    }

    private static void AppendDefaultHeaders(HttpClient httpClient, ConfigurationsOptions.Configuration configuration)
    {
        foreach (var header in configuration.Headers)
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Name, header.Value);
        }
    }

    private void DisposeHttpClientIfNotUsed(object key, object? value, EvictionReason reason, object? state)
    {
        var httpClient = (HttpClient?)value;
        if(httpClient is not null)
            httpClient.Dispose();
    }
}