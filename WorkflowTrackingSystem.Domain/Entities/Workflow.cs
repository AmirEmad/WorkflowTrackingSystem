using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class Workflow : BaseEntity
    {

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
        [JsonIgnore]
        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
    }
}
