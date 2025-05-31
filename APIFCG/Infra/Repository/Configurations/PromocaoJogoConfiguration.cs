using APIFCG.Infra.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIFCG.Infra.Repository.Configurations
{
    public class PromocaoJogoConfiguration : IEntityTypeConfiguration<PromocaoJogo>
    {
        public void Configure(EntityTypeBuilder<PromocaoJogo> builder)
        {
            builder.ToTable("PromocaoJogo");

            // Chave composta
            builder.HasKey(p => new {p.IdJogo });

            // Propriedades
            builder.Property(p => p.IdJogo).HasColumnType("INT").IsRequired();
            builder.Property(p => p.DataInicioPromocao).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.DataFimPromocao).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.ValorJogoPromocao).HasColumnType("DECIMAL(10,2)").IsRequired();

            // FK para Jogo
            builder.HasOne(p => p.Jogo)
                   .WithMany()
                   .HasForeignKey(p => p.IdJogo)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}