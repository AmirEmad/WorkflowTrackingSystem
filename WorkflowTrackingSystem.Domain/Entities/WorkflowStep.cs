using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class WorkflowStep
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; }

        [Required]
        [MaxLength(255)]
        public string StepName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AssignedTo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? NextStep { get; set; }

        public int Order { get; set; }
        public bool RequiresValidation { get; set; } = false;

        [JsonIgnore]
        public virtual Workflow Workflow { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<ProcessStep> ProcessSteps { get; set; } = new List<ProcessStep>();
    }
}
