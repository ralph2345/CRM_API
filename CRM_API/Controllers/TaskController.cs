using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using Crm.Application.DTO.Task;
using System.Security;
using Crm.Domain;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic", Policy = "ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("all-tasks")]
        public async Task<IActionResult> GetAllTasks([FromQuery] bool ascending
            ,[FromQuery] TaskFilters filters
            ,[FromQuery] int pageNumber
            ,[FromQuery] int pageSize)
        {
            var tasks = await _taskService.GetAllTask(ascending, filters, pageNumber, pageSize);
            if (tasks == null ||!tasks.Items.Any()) { return NotFound(); }
            return Ok(tasks);
        }

        [HttpGet("search-task")]
        public async Task<IActionResult> GetTaskById([FromQuery] string? name)
        {
            var task = await _taskService.SearchTask(name);
            if (task == null || !task.Any()) { return NotFound(new {Message = $"'{name}' Not Found"}); }
            return Ok(task);
        }

        [HttpGet("all-archive-tasks")]
        public async Task<IActionResult> GetAllArchiveTasks()
        {
            var tasks = await _taskService.AllArchiveTask();
            if (tasks == null || !tasks.Any()) { return NotFound(new { Message = $"No archived task found" }); }
            return Ok(tasks);
        }

        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask([FromBody] CreateTaskDto task)
        {
            var result = await _taskService.AddTask(task);
            if (result == null || !result.Any()) { return BadRequest(new {Message =$"Unable to add task please check the data"}); }
            return Created("", result);
        }

        /*[HttpPut("update-task")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDetailsDto task)
        {
            var result = await _taskService.UpdateTask(task);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }*/

        [HttpPut("is-archive-task")]
        public async Task<IActionResult> ArchiveTask([FromQuery] bool isArchived, [FromQuery] List<int> taskId)
        {
            var results = await _taskService.IsArchivedTask(isArchived, taskId);
            if(results == null) { return NotFound(new {Message = $"No task found for archival"}); }
            return Ok(results);
        }

    }
}
