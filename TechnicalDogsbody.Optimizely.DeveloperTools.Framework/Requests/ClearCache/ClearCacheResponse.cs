using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ClearCache
{
    public record ClearCacheResponse
    {
        public bool Reset { get; internal init; }
        public IEnumerable<string> Keys { get; internal init; }
    }
}
