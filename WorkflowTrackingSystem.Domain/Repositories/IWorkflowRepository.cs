using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Domain.Repositories
{
    public interface IWorkflowRepository : IBaseRepository<Workflow>
    {
        Task<bool> ExistsAsync(Guid id);
    }
}
