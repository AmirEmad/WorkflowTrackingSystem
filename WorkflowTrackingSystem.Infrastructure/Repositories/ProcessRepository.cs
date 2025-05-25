using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Data;

namespace WorkflowTrackingSystem.Infrastructure.Repositories
{
    public class ProcessRepository : BaseRepository<Process>, IProcessRepository
    {
        public ProcessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Process>> GetAllAsync(Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null)
        {
            try
            {
                var query = _context.Processes
                    .Include(p=>p.ProcessSteps)
                    .Include(x=>x.Workflow)
                    .ThenInclude(x=>x.Steps)
                    .AsQueryable();
                if (workflowId.HasValue)
                {
                    query = query.Where(p => p.WorkflowId == workflowId.Value);
                }
                if (status.HasValue)
                {
                    query = query.Where(p => p.Status == status.Value);
                }
                if (!string.IsNullOrEmpty(assignedTo))
                {
                    query = query.Where(p => p.Workflow.Steps.Any(s => s.AssignedTo.ToLower() == assignedTo));
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
