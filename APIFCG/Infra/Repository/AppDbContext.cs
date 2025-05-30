using APIFCG.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace APIFCG.Infra.Repository
{
    public class AppDbContext : DbContext
    {
        // Defina seus DbSets (tabelas)
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Jogo> Jogo { get; set; }

        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
