using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Model;
using APIFCG.Infra.Repository;
using APIFCG.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{

   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly BaseLogger<UsuarioController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(
            BaseLogger<UsuarioController> logger,
            IUsuarioRepository usuarioRepository
        )
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetTodosUsuarios()
        {
            try
            {
                _logger.LogInformation("Fetching list of users");
                var usuarios = _usuarioRepository.ObterTodos();
                return Ok(usuarios);
            }
            catch(Exception e)
            {
                return BadRequest($"Error fetching users: {e.Message}");
            }

        }

        [HttpPost]
        public IActionResult CreateUsuario(UsuarioDTO usuario)
        {
            try
            {
                _logger.LogInformation("Creating a new user");
                _usuarioRepository.Cadastrar(new Infra.Entity.Usuario
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha, // Note: In a real application, ensure to hash the password before saving
                    Lvl = 1
                });
                return StatusCode(200, "User created successfully");

            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating user: {e.Message}");
                return BadRequest($"Error creating user: {e.Message}");
            }
        }
    }
}
