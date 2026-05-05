using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.User;
using PMS.Application.Interfaces.Services;
using PMS.Application.Services.userser;

namespace PMS.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var id= await _userServices.CreateUserAsync(dto);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userServices.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userServices.GetAllAsync();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            var result = await _userServices.UpdateUserAsync(dto);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userServices.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
