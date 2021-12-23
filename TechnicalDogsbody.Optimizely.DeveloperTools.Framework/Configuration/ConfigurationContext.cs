using EPiServer.Security;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Configuration
{
    public record ConfigurationContext
    {
        public string SqlServerConnectionString { get; set; }

        public bool Enabled { private get; set; } = true;

        public bool IsEnabled()
        {
            if (PrincipalInfo.CurrentPrincipal.IsInRole("Developers"))
            {
                return Enabled;
            }

            return false;
        }
    }
}
