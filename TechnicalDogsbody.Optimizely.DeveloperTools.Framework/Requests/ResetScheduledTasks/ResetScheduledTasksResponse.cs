using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ResetScheduledTasks
{
    public record ResetScheduledTasksResponse
    {
        public bool Reset { get; internal init; }
        public IEnumerable<ScheduledTask> Tasks { get; internal init; }
    }

    public record ScheduledTask
    {
        public Guid Id { get; internal init; }
        public string Name { get; internal init; }
        public bool IsRunning { get; internal init; }
    }
}
