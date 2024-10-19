using Vivacity.Server.Factories;

namespace Vivacity.Server.Services;

public class ConfigurationLoader
{
    private readonly ConfigurationHttpClientFactory _configurationHttpClientFactory;
    private readonly FileSystem _fileSystem;
    
    public ConfigurationLoader(ConfigurationHttpClientFactory configurationHttpClientFactory, FileSystem fileSystem)
    {
        _configurationHttpClientFactory = configurationHttpClientFactory;
        _fileSystem = fileSystem;
    }
    
    public async Task<string> Load(string name, Uri configurationUri)
    {
        if (configurationUri.IsFile)
            return LoadFromFile(configurationUri);

        return await LoadFromHttpAsync(name, configurationUri);
    }
    
    public string LoadFromFile(Uri configurationUri)
    {
        return _fileSystem.ReadAllText(configurationUri.AbsoluteUri);
    }

    public async Task<string> LoadFromHttpAsync(string name, Uri configurationUri)
    {
        var httpClient = _configurationHttpClientFactory.Get(name);
        if (httpClient is null)
            httpClient = _configurationHttpClientFactory.Create(name);

        return await httpClient.GetStringAsync(configurationUri);
    }
}