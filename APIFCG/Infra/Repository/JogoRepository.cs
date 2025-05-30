using APIFCG.Infra.Entity;

namespace APIFCG.Infra.Repository
{
    public interface IJogoRepository : IRepository<Jogo>
    {

    }
    public class JogoRepository : EFRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
