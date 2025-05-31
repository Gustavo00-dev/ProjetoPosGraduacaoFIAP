using APIFCG.Infra.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIFCG.Infra.Repository.Configurations
{
    public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("Jogo");
            builder.HasKey(j => j.idJogo);
            builder.Property(j => j.idJogo).HasColumnType("INT").ValueGeneratedOnAdd();
            builder.Property(j => j.Nome).HasColumnName("Nome").IsRequired().HasMaxLength(100);
            builder.Property(j => j.NomeAbreviado).HasColumnName("NomeAbreviado").IsRequired().HasMaxLength(100);
            builder.Property(j => j.DataLancamento).HasColumnName("DataLancamento").IsRequired().HasColumnType("DATETIME");
            builder.Property(j => j.ValorVenda).HasColumnName("ValorVenda").IsRequired().HasColumnType("DECIMAL(10,2)");
            builder.Property(j => j.UsuarioResponsavelCadastro).HasColumnName("UsuarioResponsavelCadastro").IsRequired().HasMaxLength(100);
            builder.Property(j => j.DataCadastro).HasColumnName("DataCadastro").IsRequired().HasColumnType("DATETIME");
        }
    }
}
