using WorkflowTrackingSystem.Application.Services;

namespace WorkflowTrackingSystem.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IValidationService validationService)
        {
            // Log all validation attempts
            if (context.Request.Path.StartsWithSegments("/api/processes/execute"))
            {
                _logger.LogInformation("Process execution attempt at {Timestamp}", DateTime.UtcNow);

                // Enable buffering to read the request body multiple times
                context.Request.EnableBuffering();

                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                _logger.LogInformation("Execute step request body: {Body}", body);
            }

            await _next(context);
        }
    }
}
