using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Task;
using PMS.Application.Interfaces.Services;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Authorize(Roles = "User")]
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
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto,int? CategoryId)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _taskService.CreateAsync(dto,UserId,CategoryId);
            if(result==null)
                return BadRequest();
            //return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            return Ok(result);
        }//

        [HttpGet("{Taskid}")]
        public async Task<IActionResult> GetById(int Taskid)
        {
            var UserId = User.GetBusinessUserId();
            var task = await _taskService.GetByIdAsync(Taskid, UserId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }//

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var UserId = User.GetBusinessUserId();
            var tasks = await _taskService.GetByUserAsync(UserId);
            if(tasks == null)
                return NotFound();
            return Ok(tasks);
        }//

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskDto dto,int TaskId)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _taskService.UpdateAsync(dto,TaskId,UserId);
            if (!result)
                return NotFound();

            return Ok();
        }//

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int Taskid)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _taskService.DeleteAsync(Taskid, UserId);
            if (!result)
                return NotFound();

            return Ok();
        }//



        [HttpPatch("{Taskid}/status")]
        public async Task<IActionResult> ChangeStatus(int TaskId,[FromQuery] string status)
        {
           var  userId = User.GetBusinessUserId();
            var result = await _taskService.ChangeStatusAsync(TaskId, status, userId);
            if (!result)
                return BadRequest();

            return Ok();
        }//



        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] int? categoryId,[FromQuery] int? tagId,[FromQuery] DateTime? from,[FromQuery] DateTime? to)
        {
            var UserId = User.GetBusinessUserId();
            var tasks = await _taskService.FilterAsync(UserId, categoryId, tagId, from, to);
            if (tasks==null) return NotFound();
            return Ok(tasks);
        }//

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var UserId = User.GetBusinessUserId();
            var tasks = await _taskService.SearchAsync(UserId, keyword);
            if(tasks==null) return NotFound();
            return Ok(tasks);
        }//

    }
}
