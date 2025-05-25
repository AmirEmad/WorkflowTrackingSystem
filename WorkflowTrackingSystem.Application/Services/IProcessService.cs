using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Application.Services
{
    public interface IProcessService
    {
        Task<IEnumerable<Process>> GetProcessesAsync(Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null);
        Task<Process?> GetProcessByIdAsync(Guid id);
        Task<Process> StartProcessAsync(StartProcessDto dto);
        Task<Process> ExecuteStepAsync(ExecuteStepDto dto);
    }
}
