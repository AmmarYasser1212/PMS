using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Tag;
using PMS.Application.Interfaces.Services;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagServices _tagService;

        public TagsController(ITagServices tagService)
        {
            _tagService = tagService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTagDto dto)
        {
            var result = await _tagService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("{tagId}")]

        public async Task<IActionResult> GetById(  int tagId,[FromQuery] int userId) 
        {
            var tag= await _tagService.GetByIdAsync(tagId, userId);
            return Ok(tag);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagDto dto)
        {
            var result = await _tagService.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _tagService.DeleteAsync(id, userId);
            if (!result)
                return NotFound();

            return Ok();
        }

       
        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int userId)
        {
            var tags = await _tagService.GetByUserAsync(userId);
            return Ok(tags);
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignTagsToTask(int taskId, List<int> tagIds, int userId)
        {
            var result = await _tagService.AssignTagsToTask(taskId, tagIds, userId);
            if (!result)
                return BadRequest();

            return Ok();
        }


        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId, int userId)
        {
            var result = await _tagService.RemoveTagFromTask(taskId, tagId, userId);
            if (!result)
                return NotFound();

            return Ok();
        }

       
        [HttpGet("{tagId}/tasks")]
        public async Task<IActionResult> FilterTasksByTagId(int tagId, [FromQuery] int userId)
        {
            var tasks = await _tagService.FilterTasksByTag(tagId, userId);
            return Ok(tasks);

        }
}
}
