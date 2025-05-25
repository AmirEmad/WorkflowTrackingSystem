using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Domain.Repositories
{
    public interface IProcessRepository : IBaseRepository<Process>
    {
        Task<IEnumerable<Process>> GetAllAsync(Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null);
    }
}
