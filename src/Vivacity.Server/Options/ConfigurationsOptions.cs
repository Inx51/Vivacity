using System.Net;

namespace Vivacity.Server.Options;

public class ConfigurationsOptions
{
    public const string SectionName = "Configurations";
    
    public IEnumerable<Configuration> Configurations { get; set; } = [];

    public class Configuration
    {
        public required string Name { get; set; }

        public IEnumerable<HeaderOptions> Headers { get; set; } = [];

        public CredentialsOptions Credentials { get; set; }
        
        public required Uri Uri { get; set; }

        public required int PollInterval { get; set; }

        public class CredentialsOptions
        {
            public bool UseDefaultCredentials { get; set; }

            public NetworkCredentialsOptions? NetworkCredentials { get; set; }

            public class NetworkCredentialsOptions
            {
                public required string Username { get; set; }

                public required string Password { get; set; }

                public string? Domain { get; set; }
            }
        }
        
        public class HeaderOptions
        {
            public required string Name { get; set; }

            public required string Value { get; set; }
        }
    }
}