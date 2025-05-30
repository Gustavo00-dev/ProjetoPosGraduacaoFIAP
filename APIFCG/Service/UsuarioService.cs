using APIFCG.Infra.LogAPI;
using APIFCG.Infra.Model;

namespace APIFCG.Service
{
    public interface IUsuarioService {
        UsuarioDTO CadastrarUsuario(UsuarioDTO usuario);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly BaseLogger<UsuarioService> _logger;
        public UsuarioService(
            BaseLogger<UsuarioService> logger
        )
        {
            _logger = logger;
        }

        public UsuarioDTO CadastrarUsuario(UsuarioDTO usuario)
        {
            _logger.LogInformation($"Cadastrando usuário: {usuario.Nome}");
            
            return usuario;
        }
    }
}
