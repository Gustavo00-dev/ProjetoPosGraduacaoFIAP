using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Model;
using APIFCG.Infra.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIFCG.Controllers
{
    /// <summary>
    /// Controller para gerenciar jogos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly BaseLogger<UsuarioController> _logger;
        private readonly IJogoRepository _jogoRepository;
        private readonly IBibliotecaJogosClienteRepository _bibliotecaJogosClienteRepository;
        private readonly IPromocaoJogoRepository _promocaoJogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public JogoController(
            BaseLogger<UsuarioController> logger,
            IJogoRepository jogoRepository,
            IBibliotecaJogosClienteRepository bibliotecaJogosClienteRepository,
            IPromocaoJogoRepository promocaoJogoRepository,
            IUsuarioRepository usuarioRepository
        )
        {
            _logger = logger;
            _jogoRepository = jogoRepository;
            _bibliotecaJogosClienteRepository = bibliotecaJogosClienteRepository;
            _promocaoJogoRepository = promocaoJogoRepository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Listar todos os jogos.
        /// </summary>
        /// <returns>Todos os jogos disponiveis</returns>
        [HttpGet("BuscarTodosJogos")]
        public IActionResult GetTodosJogos()
        {
            try
            {
                _logger.LogInformation("Listar todos jogos.");
                var jogos = _jogoRepository.ObterTodos();
                return Ok(jogos);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao listar todos os jogos: {e.Message}");
            }
        }

        /// <summary>
        /// Listar todos os jogos de um cliente específico.
        /// </summary>
        /// <param name="idtCliente"></param>
        /// <returns></returns>
        [HttpGet("ListarTodosJogosCliente")]
        public IActionResult ListarJogosCliete(int idtCliente)
        {
            try
            {
                _logger.LogInformation($"Listando jogos do cliente {idtCliente}.");
                var jogos = _bibliotecaJogosClienteRepository.ObterJogosPorUsuario(idtCliente);
                if (jogos == null || !jogos.Any())
                {
                    _logger.LogWarning($"Nenhum jogo encontrado para o cliente {idtCliente}.");
                    return NotFound("Nenhum jogo encontrado para este cliente.");
                }
                return Ok(jogos);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao listar jogos do cliente: {e.Message}");
                return BadRequest($"Erro ao listar jogos do cliente: {e.Message}");
            }
        }
        /// <summary>
        /// Endpoint para cadastrar um novo jogo.
        /// </summary>
        /// <param name="jogoDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("CadastrarNovoJogo")]
        public IActionResult CadastrarJogo(JogoDTO jogoDTO)
        {
            try
            {
                _logger.LogInformation("Cadastrando novo jogo.");
                if (string.IsNullOrEmpty(jogoDTO.Nome) || jogoDTO.ValorVenda <= 0)
                {
                    _logger.LogWarning("Tentativa de cadastro de jogo inválida: dados incompletos.");
                    return BadRequest("Dados inválidos para cadastro de jogo.");
                }

                var jogo = new Infra.Entity.Jogo
                {
                    Nome = jogoDTO.Nome,
                    NomeAbreviado = jogoDTO.NomeAbreviado,
                    DataLancamento = jogoDTO.DataLancamento,
                    ValorVenda = jogoDTO.ValorVenda,
                    UsuarioResponsavelCadastro = jogoDTO.UsuarioResponsavelCadastro ?? "Admin", // Defult para Admin
                    DataCadastro = DateTime.Now

                };
                _jogoRepository.Cadastrar(jogo);
                _logger.LogInformation($"Jogo {jogo.Nome} cadastrado com sucesso.");
                return StatusCode(201, $"Jogo {jogo.Nome} cadastrado com sucesso.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao cadastrar jogo: {e.Message}");
                return BadRequest($"Erro ao cadastrar jogo: {e.Message}");
            }
        }

        /// <summary>
        /// Endpoint para cadastrar uma nova promoção de jogo.
        /// </summary>
        /// <param name="promocaoDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("CadastrarPromoçoes")]
        public IActionResult CadastrarPromocao(PromocaoJogoDTO promocaoDTO)
        {
            try
            {
                _logger.LogInformation("Cadastrando promoção de jogo.");
                if (promocaoDTO.IdJogo <= 0)
                {
                    _logger.LogWarning("Tentativa de cadastro de promoção inválida: ID do jogo inválido.");
                    return BadRequest("Dados inválidos para cadastro de promoção.");
                }
                var jogo = _jogoRepository.ObterPorId(promocaoDTO.IdJogo);
                if (jogo == null)
                {
                    _logger.LogWarning($"Jogo com ID {promocaoDTO.IdJogo} não encontrado.");
                    return NotFound("Jogo não encontrado.");
                }

                // Verifica se já existe uma promoção ativa para o jogo
                var promocaoExistente = _promocaoJogoRepository.ObterPorId(promocaoDTO.IdJogo);
                if (promocaoExistente != null && promocaoExistente.DataFimPromocao >= DateTime.Now)
                {
                    _logger.LogWarning($"Promoção já existe para o jogo {jogo.Nome}.");
                    return BadRequest("Já existe uma promoção ativa para este jogo.");
                }
                // Cadastra a nova promoção
                var promocao = new Infra.Entity.PromocaoJogo
                {
                    IdJogo = promocaoDTO.IdJogo,
                    DataInicioPromocao = promocaoDTO.DataInicioPromocao,
                    DataFimPromocao = promocaoDTO.DataFimPromocao,
                    ValorJogoPromocao = promocaoDTO.ValorJogoPromocao
                };

                _promocaoJogoRepository.Cadastrar(promocao);

                _logger.LogInformation($"Promoção cadastrada com sucesso para o jogo {jogo.Nome}.");
                return StatusCode(201, $"Promoção cadastrada com sucesso para o jogo {jogo.Nome}.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao cadastrar promoção: {e.Message}");
                return BadRequest($"Erro ao cadastrar promoção: {e.Message}");
            }
        }

        /// <summary>
        /// Endpoint para comprar um jogo.
        /// </summary>
        /// <returns>Status da Compra</returns>
        [HttpPost("ComprarJogo")]
        public IActionResult ComprarJogo(int idUsuario, int idJogo)
        {
            try
            {
                _logger.LogInformation("Comprando jogo.");
                if (idUsuario <= 0 || idJogo <= 0)
                {
                    _logger.LogWarning("Tentativa de compra de jogo inválida: ID do usuário ou do jogo inválido.");
                    return BadRequest("Dados inválidos para compra de jogo.");
                }

                var jogo = _jogoRepository.ObterPorId(idJogo);
                if (jogo == null)
                {
                    _logger.LogWarning($"Jogo com ID {idJogo} não encontrado.");
                    return NotFound("Jogo não encontrado.");
                }

                //Validar se jogo esta em promocao
                var promocao = _promocaoJogoRepository.ObterPorId(idJogo);
                if (promocao != null && promocao.DataFimPromocao >= DateTime.Now)
                {
                    jogo.ValorVenda -= promocao.ValorJogoPromocao;
                }

                _bibliotecaJogosClienteRepository.Cadastrar(new Infra.Entity.BibliotecaJogosCliente
                {
                    IdUsuario = idUsuario,
                    IdJogo = idJogo,
                    DataCompra = DateTime.Now,
                    ValorCompra = jogo.ValorVenda
                });
                _logger.LogInformation($"Jogo {jogo.Nome} comprado com sucesso por usuário {idUsuario}.");
                return StatusCode(200, $"Jogo {jogo.Nome} comprado com sucesso.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao comprar jogo: {e.Message}");
                return BadRequest($"Erro ao comprar jogo: {e.Message}");
            }
        }
    }
}
