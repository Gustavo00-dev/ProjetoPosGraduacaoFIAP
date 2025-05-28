using APIFCG.Infra.LogAPI;
using APIFCG.Service;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{
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

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            // Simulate fetching users from a database or service
            _logger.LogInformation("Fetching list of users");
            var usuarios = new List<string> { "Usuario1", "Usuario2", "Usuario3" };
            return Ok(usuarios);
        }
    }
}
