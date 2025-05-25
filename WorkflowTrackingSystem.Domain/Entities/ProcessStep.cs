using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class ProcessStep : BaseEntity
    {
        public Guid ProcessId { get; set; }
        public Guid WorkflowStepId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string? PerformedBy { get; set; }
        public string? Action { get; set; }
        public StepStatus Status { get; set; } = StepStatus.Pending;
        public string? ValidationResult { get; set; }
        public string? Comments { get; set; }
        public DateTime? CompletedAt { get; set; }
        [JsonIgnore]
        public virtual Process Process { get; set; } = null!;
        public virtual WorkflowStep WorkflowStep { get; set; } = null!;
    }
}
