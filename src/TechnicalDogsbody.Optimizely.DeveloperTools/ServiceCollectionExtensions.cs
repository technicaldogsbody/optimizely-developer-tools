using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Cms.Shell;
using EPiServer.Security;
using EPiServer.Shell.Modules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TechnicalDogsbody.Optimizely.DeveloperTools.Configuration;
using TechnicalDogsbody.Optimizely.DeveloperTools.Contracts;
using TechnicalDogsbody.Optimizely.DeveloperTools.Factories;
using TechnicalDogsbody.Optimizely.DeveloperTools.Providers;
using TechnicalDogsbody.Optimizely.DeveloperTools.Validators;

namespace TechnicalDogsbody.Optimizely.DeveloperTools
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDeveloperTools(
            this IServiceCollection services,
            Action<ConfigurationContext> setup)
        {
            if (setup == null) throw new ArgumentNullException(nameof(setup));

            var context = new ConfigurationContext();
            setup.Invoke(context);
            var validator = new ConfigurationContextValidator();
            validator.ValidateAndThrowAsync(context);

            services.AddScoped(_ => context);

            IDbConnectionFactory connection = new SqlServerConnectionFactory(context.SqlServerConnectionString);
            services.AddTransient(_ => connection);

            services.AddCmsUI();
            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i => i.Name.Equals(Constants.ModuleName, StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new ModuleDetails { Name = Constants.ModuleName });
                    }
                });

            return services;
        }
    }
}
