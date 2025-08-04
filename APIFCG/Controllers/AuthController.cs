﻿using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Model;
using APIFCG.Infra.Repository;
using APIFCG.Service;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly BaseLogger<AuthController> _logger;

        public AuthController(
            IJwtService jwtService,
            IUsuarioRepository usuarioRepository,
            BaseLogger<AuthController> logger
        )
        {
            _jwtService = jwtService;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        /// <summary>
        /// Endpoit para autenticação de usuários.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Token</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                {
                    _logger.LogWarning("Tentativa de login inválida: nome de usuário ou senha invalidos.");
                    return BadRequest("Tentativa de Login Invalida.");
                }

                var usuario = _usuarioRepository.ObterTodos()
                    .FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Password);
                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario ou senha não existe.");
                    return Unauthorized("Senha ou Usuario invalido.");
                }
                // Define o papel do usuário com base no nível de acesso - 1 Usuario comum, 2 Admin.
                var role = usuario.Lvl == 2 ? "Admin" : "User";
                var token = _jwtService.GenerateToken(login.Email, role);

                return Ok(new { token });
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Endpoint apenas para testes
        /// </summary>
        /// <returns>Mensagem de sucesso</returns>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Autenticação funcionando corretamente.");
        }
    }
}
