using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset
{
    public record ModelResetResponse
    {
        public bool Reset { get; internal init; }
        public string Version { get; internal init; }
        public IEnumerable<ContentType> ContentTypes { get; internal init; }
    }

    public record ContentType
    {
        public int Id { get; internal init; }
        public string Name { get; internal init; }
        public string ModelType { get; internal init; }
    }
}
