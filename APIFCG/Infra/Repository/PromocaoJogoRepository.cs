using APIFCG.Infra.Entity;

namespace APIFCG.Infra.Repository
{
    public interface IPromocaoJogoRepository : IRepository<PromocaoJogo>
    {

    }
    public class PromocaoJogoRepository : EFRepository<PromocaoJogo>, IPromocaoJogoRepository
    {
        public PromocaoJogoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
