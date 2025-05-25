using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Application.Services;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _processService;

        public ProcessesController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProcesses(
            [FromQuery] Guid? workflowId = null,
            [FromQuery] ProcessStatus? status = null,
            [FromQuery] string? assignedTo = null)
        {
            var processes = await _processService.GetProcessesAsync(workflowId, status, assignedTo);
            return Ok(processes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcess(Guid id)
        {
            var process = await _processService.GetProcessByIdAsync(id);
            if (process == null)
                return NotFound();

            return Ok(process);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartProcess([FromBody] StartProcessDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var process = await _processService.StartProcessAsync(dto);
                return CreatedAtAction(nameof(GetProcess), new { id = process.Id }, process);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteStep([FromBody] ExecuteStepDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var process = await _processService.ExecuteStepAsync(dto);
                return Ok(process);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
