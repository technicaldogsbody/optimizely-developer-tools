using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset
{
    public record ModelResetRequest : IRequest<ModelResetResponse>
    {
        public bool Reset { get; init; }

        public IEnumerable<int> Ids { get; init; }
    }
}
