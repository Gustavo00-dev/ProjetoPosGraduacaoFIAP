using APIFCG.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace APIFCG.Infra.Repository
{
    public interface IBibliotecaJogosClienteRepository : IRepository<BibliotecaJogosCliente>
    {
        IList<Jogo> ObterJogosPorUsuario(int idUsuario);
    }

    public class BibliotecaJogosClienteRepository : EFRepository<BibliotecaJogosCliente>, IBibliotecaJogosClienteRepository
    {
        public BibliotecaJogosClienteRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retorna todos os jogos que o cliente possui.
        /// </summary>
        /// <param name="idUsuario">ID do cliente</param>
        /// <returns>Lista de jogos do cliente</returns>
        public IList<Jogo> ObterJogosPorUsuario(int idUsuario)
        {
            return _dbSet
                .Include(b => b.Jogo)
                .Where(b => b.IdUsuario == idUsuario)
                .Select(b => b.Jogo)
                .ToList();
        }
    }
}
