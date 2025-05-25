using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class Process : BaseEntity
    {
        public Guid WorkflowId { get; set; }
        public string Initiator { get; set; } = string.Empty;
        public ProcessStatus Status { get; set; } = ProcessStatus.Active;
        public string? CurrentStep { get; set; }

        public virtual Workflow Workflow { get; set; } = null!;
        public virtual ICollection<ProcessStep> ProcessSteps { get; set; } = new List<ProcessStep>();
    }
}
