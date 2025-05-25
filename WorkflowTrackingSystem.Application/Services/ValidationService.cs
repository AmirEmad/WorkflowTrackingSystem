using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Repositories;

namespace WorkflowTrackingSystem.Application.Services
{

    public class ValidationService : IValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(HttpClient httpClient, ILogger<ValidationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool IsValid, string Message)> ValidateStepAsync(string stepName, string action, object? data = null)
        {
            try
            {
                // Simulate external API validation
                await Task.Delay(500); // Simulate network delay

                // Example validation logic
                if (stepName.Contains("Finance", StringComparison.OrdinalIgnoreCase))
                {
                    // Simulate financial validation
                    var random = new Random();
                    var isValid = random.Next(1, 10) > 2; // 80% success rate

                    _logger.LogInformation("Financial validation for step {StepName} with action {Action}: {Result}",
                        stepName, action, isValid ? "Success" : "Failed");

                    return isValid
                        ? (true, "Financial validation successful")
                        : (false, "Financial validation failed - insufficient budget");
                }

                // Default validation passes
                return (true, "Validation successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for step {StepName}", stepName);
                return (false, "Validation service unavailable");
            }
        }
    }
}
