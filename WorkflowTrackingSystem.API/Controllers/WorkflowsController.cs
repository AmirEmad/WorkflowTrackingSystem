using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Application.Services;

namespace WorkflowTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowsController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkflows()
        {
            var workflows = await _workflowService.GetAllWorkflowsAsync();
            return Ok(workflows);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(Guid id)
        {
            var workflow = await _workflowService.GetWorkflowByIdAsync(id);
            if (workflow == null)
                return NotFound();

            return Ok(workflow);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workflow = await _workflowService.CreateWorkflowAsync(dto);
            return CreatedAtAction(nameof(GetWorkflow), new { id = workflow.Id }, workflow);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkflow(Guid id, [FromBody] CreateWorkflowDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var workflow = await _workflowService.UpdateWorkflowAsync(id, dto);
                return Ok(workflow);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflow(Guid id)
        {
            await _workflowService.DeleteWorkflowAsync(id);
            return NoContent();
        }
    }
}
