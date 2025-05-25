using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Domain.Enums
{
    public enum StepStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Rejected = 4,
        ValidationFailed = 5
    }
}
