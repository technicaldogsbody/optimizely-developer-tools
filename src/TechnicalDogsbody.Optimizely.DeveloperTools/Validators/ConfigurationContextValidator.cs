using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TechnicalDogsbody.Optimizely.DeveloperTools.Configuration;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Validators
{
    public class ConfigurationContextValidator : AbstractValidator<ConfigurationContext>
    {
        public ConfigurationContextValidator()
        {
            RuleFor(context => context.SqlServerConnectionString).NotNull().NotEmpty();
        }
    }
}
