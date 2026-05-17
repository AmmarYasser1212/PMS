using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Category;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var UserId = User.GetBusinessUserId();
            var id = await _service.CreateAsync(dto, UserId);
            return Ok(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var UserId = User.GetBusinessUserId();
            var data = await _service.GetByUserAsync(UserId);
            return Ok(data);
        }


        [HttpGet("{CategoryId}")]
        public async Task<IActionResult> Get(int CategoryId)
        {
            var UserId = User.GetBusinessUserId();
            var data = await _service.GetByIdAsync(CategoryId, UserId);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPut("{CategoryId}")]
        public async Task<IActionResult> Update(UpdateCategory dto,int CategoryId)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _service.UpdateAsync(dto,CategoryId,UserId);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _service.DeleteAsync(id, UserId);
            if (!result)
                return NotFound();

            return Ok();
        }

    }
}
