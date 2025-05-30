using APIFCG.Infra.Entity;

namespace APIFCG.Infra.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        
    }
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {

        }
    }
}
