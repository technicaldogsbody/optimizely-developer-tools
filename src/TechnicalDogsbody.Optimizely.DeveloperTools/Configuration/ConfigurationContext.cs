using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Security;
using Microsoft.Identity.Client;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Configuration
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
