using APIFCG.Infra.Model;
using APIFCG.Service;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            // Validação temporaria
            if (login.Username == "admin" && login.Password == "1234")
            {
                var token = _jwtService.GenerateToken(login.Username, "Admin");
                return Ok(new { token });
            }
            else if (login.Username == "user" && login.Password == "1234")
            {
                var token = _jwtService.GenerateToken(login.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
