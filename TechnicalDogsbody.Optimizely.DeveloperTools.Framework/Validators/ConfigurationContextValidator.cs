using FluentValidation;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Configuration;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Validators
{
    public class ConfigurationContextValidator : AbstractValidator<ConfigurationContext>
    {
        public ConfigurationContextValidator()
        {
            RuleFor(context => context.SqlServerConnectionString).NotNull().NotEmpty();
        }
    }
}
