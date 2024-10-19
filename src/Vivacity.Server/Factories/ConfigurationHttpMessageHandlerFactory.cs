using System.Net;
using Microsoft.Extensions.Options;
using Vivacity.Server.Options;
using Vivacity.Server.Extensions;

namespace Vivacity.Server.Factories;

public class ConfigurationHttpMessageHandlerFactory
{
    private readonly IOptionsSnapshot<Options.ConfigurationsOptions> _configurations;
    
    public ConfigurationHttpMessageHandlerFactory(IOptionsSnapshot<Options.ConfigurationsOptions> configurations)
    {
        _configurations = configurations;
    }

    public HttpMessageHandler Create(string name)
    {
        var configuration = _configurations.Value.GetConfigurationByName(name);

        var handler = CreateDefaultHandler();
        
        if(configuration!.Credentials?.UseDefaultCredentials is not null)
            handler.Credentials = CredentialCache.DefaultCredentials;

        if (configuration.Credentials?.NetworkCredentials is not null)
            handler.Credentials = new NetworkCredential
            {
                UserName = configuration.Credentials?.NetworkCredentials.Username,
                Password = configuration.Credentials?.NetworkCredentials.Password,
                Domain = configuration.Credentials?.NetworkCredentials.Domain
            };

        return handler;
    }

    private SocketsHttpHandler CreateDefaultHandler() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(15)
    };
}