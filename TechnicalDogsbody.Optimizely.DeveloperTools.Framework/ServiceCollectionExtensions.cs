using System;
using System.Linq;
using System.Reflection;
using EPiServer.Cms.Shell;
using EPiServer.Shell.Modules;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Configuration;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Factories;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Validators;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDeveloperTools(
            this IServiceCollection services,
            Action<ConfigurationContext> setup)
        {
            if (setup == null) throw new ArgumentNullException(nameof(setup));

            services.AddTransient(typeof(IDeveloperToolsLogger<>), typeof(DeveloperToolsLogger<>));
            services.AddMediatR(Assembly.GetAssembly(typeof(ServiceCollectionExtensions)));

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
