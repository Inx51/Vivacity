namespace Vivacity.Server.Extensions;

public static class ConfigurationsOptions
{
    public static Options.ConfigurationsOptions.Configuration? GetConfigurationByName(this Options.ConfigurationsOptions configurationsOptions, string name)
    {
        return configurationsOptions.Configurations.FirstOrDefault(c => c.Name == name);
    }
}