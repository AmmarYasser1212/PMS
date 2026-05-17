using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Tag;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Authorize(Roles = "User")]
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
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.CreateAsync(dto,UserId);
            return Ok(result);
        }

        [HttpGet("{tagId}")]

        public async Task<IActionResult> GetById(  int tagId/*,[FromQuery] int userId*/) 
        {
            var UserId = User.GetBusinessUserId();
            var tag= await _tagService.GetByIdAsync(tagId, UserId);
            return Ok(tag);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int TagId,UpdateTagDto dto)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.UpdateAsync( dto, TagId, UserId);
            if (!result)
                return NotFound();

            return Ok();
        }


        [HttpDelete("{Tagid}")]
        public async Task<IActionResult> Delete(int Tagid)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.DeleteAsync(Tagid, UserId);
            if (!result)
                return NotFound();

            return Ok();
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var UserId = User.GetBusinessUserId();
            var tags = await _tagService.GetByUserAsync(UserId);
            return Ok(tags);
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignTagsToTask(int taskId, List<int> tagIds)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.AssignTagsToTask(taskId, tagIds, UserId);
            if (!result)
                return BadRequest();

            return Ok();
        }


        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.RemoveTagFromTask(taskId, tagId, UserId);
            if (!result)
                return NotFound();

            return Ok();
        }

       
        [HttpGet("{tagId}/tasks")]
        public async Task<IActionResult> FilterTasksByTagId(int tagId)
        {
            var UserId = User.GetBusinessUserId();
            var tasks = await _tagService.FilterTasksByTag(tagId, UserId);
            return Ok(tasks);

        }
}
}
