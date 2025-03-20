using AuthenticationService.DTOs;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _config = configuration;
        }


        [HttpPost("Register")]

        public async Task<ActionResult> RegisterUser([FromForm] RegisterDTO registerDTO)
        {
            try {
                var result = await _authService.CreateAccount(registerDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw new Exception(ex.Message);
              
            }
            }


        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser( [FromForm] LoginDTO loginDTO)
        {
            var result = await _authService.UserLogin(loginDTO);

            var token = result.Token;

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Authentication failed, token is null or empty.");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(300)
            };

            Response.Cookies.Append("AuthToken", token, cookieOptions);
            return Ok(result);
        }
    }
}
