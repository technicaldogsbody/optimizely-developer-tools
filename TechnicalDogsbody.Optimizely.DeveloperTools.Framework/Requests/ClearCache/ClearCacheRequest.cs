using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ClearCache
{
    public record ClearCacheRequest : IRequest<ClearCacheResponse>
    {
        public bool Reset { get; init; }
    }
}
