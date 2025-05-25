using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Application.DTOs
{
    public class StartProcessDto
    {
        [Required]
        public Guid WorkflowId { get; set; }

        [Required]
        public string Initiator { get; set; } = string.Empty;
    }
}
