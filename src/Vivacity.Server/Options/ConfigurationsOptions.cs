namespace Vivacity.Server.Options;

public class ConfigurationsOptions
{
    public List<ConfigurationOptions> Configurations { get; set; } = [];

    public class ConfigurationOptions
    {
        public required string Name { get; set; }

        public required Uri Uri { get; set; }

        public required int PollInterval { get; set; }
    }
}