using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Task;
using PMS.Application.Interfaces.Services;

namespace PMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var result = await _taskService.CreateAsync(dto);
            //return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int userId)
        {
            var task = await _taskService.GetByIdAsync(id, userId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int userId)
        {
            var tasks = await _taskService.GetByUserAsync(userId);
            return Ok(tasks);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskDto dto)
        {
            var result = await _taskService.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _taskService.DeleteAsync(id, userId);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(
    int id,
    [FromQuery] string status,
    [FromQuery] int userId)
        {
            var result = await _taskService.ChangeStatusAsync(id, status, userId);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
    [FromQuery] int userId,
    [FromQuery] int? categoryId,
    [FromQuery] int? tagId,
    [FromQuery] DateTime? from,
    [FromQuery] DateTime? to)
        {
            var tasks = await _taskService.FilterAsync(userId, categoryId, tagId, from, to);
            return Ok(tasks);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
    [FromQuery] int userId,
    [FromQuery] string keyword)
        {
            var tasks = await _taskService.SearchAsync(userId, keyword);
            return Ok(tasks);
        }

    }
}
