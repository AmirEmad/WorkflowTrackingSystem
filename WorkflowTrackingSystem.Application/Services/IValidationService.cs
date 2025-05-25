using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Application.Services
{
    public interface IValidationService
    {
        Task<(bool IsValid, string Message)> ValidateStepAsync(string stepName, string action, object? data = null);
    }
}
