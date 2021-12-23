using System.Collections.Generic;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Configuration;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Providers
{
    [MenuProvider]
    public class DeveloperToolsMenuProvider : IMenuProvider
    {
        private readonly ConfigurationContext _configContext;

        public DeveloperToolsMenuProvider(ConfigurationContext configContext)
        {
            _configContext = configContext;
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            var url = Paths.ProtectedRootPath.ToLower() + "developer-tools";

            return new MenuItem[]
            {
                new UrlMenuItem("Developer Tools", MenuPaths.Global + "/cms/developertools", url)
                {
                    IsAvailable = context => _configContext.IsEnabled()
                },
                new UrlMenuItem("Summary", MenuPaths.Global + "/cms/developertools/index", url + "/")
                {
                    IsAvailable = context => _configContext.IsEnabled()
                },
                new UrlMenuItem("Clear Cache", MenuPaths.Global + "/cms/developertools/clearcache", url + "/clearcache")
                {
                    IsAvailable = context => _configContext.IsEnabled()
                },
                new UrlMenuItem("Reset Scheduled Tasks", MenuPaths.Global + "/cms/developertools/resetscheduledtasks", url + "/resetscheduledtasks")
                {
                    IsAvailable = context => _configContext.IsEnabled()
                },
                new UrlMenuItem("Model Reset", MenuPaths.Global + "/cms/developertools/modelreset", url + "/modelreset")
                {
                    IsAvailable = context => _configContext.IsEnabled()
                },
                new UrlMenuItem("Restart Application", MenuPaths.Global + "/cms/developertools/restartapplication", url + "/restartapplication")
                {
                    IsAvailable = context => _configContext.IsEnabled()
                }
            };
        }
    }
}
