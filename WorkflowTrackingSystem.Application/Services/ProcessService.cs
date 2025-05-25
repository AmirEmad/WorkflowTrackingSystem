using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Domain.Repositories;

namespace WorkflowTrackingSystem.Application.Services
{

    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IValidationService _validationService;

        public ProcessService(
            IProcessRepository processRepository,
            IWorkflowRepository workflowRepository,
            IValidationService validationService)
        {
            _processRepository = processRepository;
            _workflowRepository = workflowRepository;
            _validationService = validationService;
        }

        public async Task<IEnumerable<Process>> GetProcessesAsync(Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null)
        {
            return await _processRepository.GetAllAsync(workflowId, status, assignedTo);
        }

        public async Task<Process?> GetProcessByIdAsync(Guid id)
        {
            return await _processRepository.FindAsync(x => x.Id == id, includes: new[] { "ProcessSteps","Workflow" });
        }

        public async Task<Process> StartProcessAsync(StartProcessDto dto)
        {
            var workflow = await _workflowRepository.FindAsync(x=>x.Id== dto.WorkflowId, includes: new[] { "Steps" });
            if (workflow == null)
                throw new ArgumentException("Workflow not found");

            var firstStep = workflow.Steps.OrderBy(s => s.Order).FirstOrDefault();

            var process = new Process
            {
                WorkflowId = dto.WorkflowId,
                Initiator = dto.Initiator,
                CurrentStep = firstStep?.StepName,
                Status = ProcessStatus.Active
            };

            if (firstStep != null)
            {
                process.ProcessSteps.Add(new ProcessStep
                {
                    ProcessId = process.Id,
                    WorkflowStepId = firstStep.Id,
                    StepName = firstStep.StepName,
                    Status = StepStatus.Pending
                });
            }

            return await _processRepository.AddAsync(process);
        }

        public async Task<Process> ExecuteStepAsync(ExecuteStepDto dto)
        {
            var process = await _processRepository.FindAsync(x=>x.Id== dto.ProcessId, new[] { "ProcessSteps", "ProcessSteps.WorkflowStep","Workflow.Steps" } );
            if (process == null)
                throw new ArgumentException("Process not found");

            var currentProcessStep = process.ProcessSteps
                .FirstOrDefault(ps => ps.StepName == dto.StepName && ps.Status == StepStatus.Pending);

            if (currentProcessStep == null)
                throw new ArgumentException("Step not found or already completed");

            // Validate if required
            if (currentProcessStep.WorkflowStep.RequiresValidation)
            {
                var (isValid, message) = await _validationService.ValidateStepAsync(dto.StepName, dto.Action);
                if (!isValid)
                {
                    currentProcessStep.Status = StepStatus.ValidationFailed;
                    currentProcessStep.ValidationResult = message;
                    _processRepository.Update(process);
                    throw new InvalidOperationException($"Validation failed: {message}");
                }
            }

            // Execute step
            currentProcessStep.PerformedBy = dto.PerformedBy;
            currentProcessStep.Action = dto.Action;
            currentProcessStep.Comments = dto.Comments;
            currentProcessStep.CompletedAt = DateTime.UtcNow;
            currentProcessStep.Status = dto.Action.ToLower() == "reject" ? StepStatus.Rejected : StepStatus.Completed;

            // Handle next step
            if (dto.Action.ToLower() == "reject")
            {
                process.Status = ProcessStatus.Rejected;
            }
            else
            {
                var nextStepName = currentProcessStep.WorkflowStep.NextStep;
                if (nextStepName == "Completed" || string.IsNullOrEmpty(nextStepName))
                {
                    process.Status = ProcessStatus.Completed;
                }
                else
                {
                    var nextWorkflowStep = process.Workflow.Steps.FirstOrDefault(s => s.StepName == nextStepName);
                    if (nextWorkflowStep != null)
                    {
                        process.ProcessSteps.Add(new ProcessStep
                        {
                            ProcessId = process.Id,
                            WorkflowStepId = nextWorkflowStep.Id,
                            StepName = nextWorkflowStep.StepName,
                            Status = StepStatus.Pending
                        });
                        process.CurrentStep = nextStepName;
                    }
                }
            }

            process.UpdatedAt = DateTime.UtcNow;
            return _processRepository.Update(process);
        }
    }
}
