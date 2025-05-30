using System.ComponentModel.DataAnnotations;

namespace APIFCG.Infra.Entity
{
    public class Usuario
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Lvl { get; set; }
    }
}
