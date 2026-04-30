using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Category;
using PMS.Application.Interfaces.Services;

namespace PMS.Controllers
{
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
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int userId)
        {
            var data = await _service.GetByUserAsync(userId);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] int userId)
        {
            var data = await _service.GetByIdAsync(id, userId);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _service.DeleteAsync(id, userId);
            if (!result)
                return NotFound();

            return Ok();
        }

    }
}
