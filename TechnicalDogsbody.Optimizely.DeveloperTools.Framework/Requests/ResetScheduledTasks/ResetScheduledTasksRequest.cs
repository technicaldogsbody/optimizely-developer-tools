using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ResetScheduledTasks
{
    public class ResetScheduledTasksRequest : IRequest<ResetScheduledTasksResponse>
    {
        public bool Reset { get; init; }

        public IEnumerable<Guid> Ids { get; init; }
    }
}
