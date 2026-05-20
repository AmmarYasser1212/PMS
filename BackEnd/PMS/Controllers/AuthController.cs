using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PMS.Application.DTO.Auth;
using PMS.Application.Interfaces.Services;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetTokenInCookie(result.Token, result.ExpiresOn);//
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);//


            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            SetTokenInCookie(result.Token,result.ExpiresOn /*DateTime.UtcNow.AddDays(7)*/);

            return Ok();
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);


            SetTokenInCookie(result.Token, result.ExpiresOn /*DateTime.UtcNow.AddDays(7)*/);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);


            return Ok();
        }
        [Authorize(Roles = "User")]
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken(/*[FromBody] RevokeToken model*/)
        {
            var userId = User.GetBusinessUserId();

            var result = await _authService.RevokeTokenAsync(userId);

            if (!result)
                return BadRequest("Token is invalid!");

            // 🧹 clear cookies
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("refreshToken");

            return Ok("Logged out successfully");
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            if (expires <= DateTime.UtcNow)
                expires = DateTime.UtcNow.AddDays(7);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
             private void SetTokenInCookie(string token, DateTime expires)
        {
            if (expires <= DateTime.UtcNow)
                expires = DateTime.UtcNow.AddMinutes(15);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("Token",token, cookieOptions);
        }
    }
    }

