using APIFCG.Infra.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIFCG.Infra.Repository.Configurations
{
    public class BibliotecaJogosClienteConfiguration : IEntityTypeConfiguration<BibliotecaJogosCliente>
    {
        public void Configure(EntityTypeBuilder<BibliotecaJogosCliente> builder)
        {
            builder.ToTable("BibliotecaJogosCliente");

            // Chave composta
            builder.HasKey(b => new { b.IdUsuario, b.IdJogo });

            // Propriedades
            builder.Property(b => b.IdUsuario).HasColumnType("INT").IsRequired();
            builder.Property(b => b.IdJogo).HasColumnType("INT").IsRequired();
            builder.Property(b => b.DataCompra).HasColumnType("DATETIME").IsRequired();
            builder.Property(b => b.ValorCompra).HasColumnType("DECIMAL(10,2)").IsRequired();

            // FK para Usuario
            builder.HasOne(b => b.Usuario)
                   .WithMany()
                   .HasForeignKey(b => b.IdUsuario)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK para Jogo
            builder.HasOne(b => b.Jogo)
                   .WithMany()
                   .HasForeignKey(b => b.IdJogo)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
