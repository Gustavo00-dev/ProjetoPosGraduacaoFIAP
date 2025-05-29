using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Model;
using APIFCG.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly BaseLogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(
            BaseLogger<UsuarioController> logger,
            IUsuarioService usuarioService
        )
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetTodosUsuarios()
        {
            
            _logger.LogInformation("Fetching list of users");
            var usuarios = new List<string> { "Usuario1", "Usuario2", "Usuario3" };
            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult CreateUsuario(UsuarioDTO usuario)
        {
            
            _logger.LogInformation("Creating a new user");
            
            return StatusCode(200, "User created successfully");
        }
    }
}
