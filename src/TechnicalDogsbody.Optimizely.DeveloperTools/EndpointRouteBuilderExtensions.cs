using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace TechnicalDogsbody.Optimizely.DeveloperTools
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapDevelopertToolsAdminUI(
            this IEndpointRouteBuilder builder)
        {
            builder.MapControllerRoute("developer tools","episerver/developer-tools/{action}", new { controller = "DeveloperTools", action = "Index"});
            return builder;
        }
    }
}
