﻿// <auto-generated />
using System;
using APIFCG.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIFCG.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250602224341_FirstMigrations")]
    partial class FirstMigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("APIFCG.Infra.Entity.BibliotecaJogosCliente", b =>
                {
                    b.Property<int>("IdUsuario")
                        .HasColumnType("INT")
                        .HasColumnOrder(0);

                    b.Property<int>("IdJogo")
                        .HasColumnType("INT")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("DataCompra")
                        .HasColumnType("DATETIME");

                    b.Property<decimal>("ValorCompra")
                        .HasColumnType("DECIMAL(10,2)");

                    b.HasKey("IdUsuario", "IdJogo");

                    b.HasIndex("IdJogo");

                    b.ToTable("BibliotecaJogosCliente", (string)null);
                });

            modelBuilder.Entity("APIFCG.Infra.Entity.Jogo", b =>
                {
                    b.Property<int>("idJogo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DataCadastro");

                    b.Property<DateTime>("DataLancamento")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DataLancamento");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Nome");

                    b.Property<string>("NomeAbreviado")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NomeAbreviado");

                    b.Property<string>("UsuarioResponsavelCadastro")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UsuarioResponsavelCadastro");

                    b.Property<decimal>("ValorVenda")
                        .HasColumnType("DECIMAL(10,2)")
                        .HasColumnName("ValorVenda");

                    b.HasKey("idJogo");

                    b.ToTable("Jogo", (string)null);
                });

            modelBuilder.Entity("APIFCG.Infra.Entity.PromocaoJogo", b =>
                {
                    b.Property<int>("IdJogo")
                        .HasColumnType("INT")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("DataFimPromocao")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataInicioPromocao")
                        .HasColumnType("DATETIME");

                    b.Property<decimal>("ValorJogoPromocao")
                        .HasColumnType("DECIMAL(10,2)");

                    b.HasKey("IdJogo");

                    b.ToTable("PromocaoJogo", (string)null);
                });

            modelBuilder.Entity("APIFCG.Infra.Entity.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Email");

                    b.Property<int>("Lvl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("Lvl");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Nome");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Senha");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("APIFCG.Infra.Entity.BibliotecaJogosCliente", b =>
                {
                    b.HasOne("APIFCG.Infra.Entity.Jogo", "Jogo")
                        .WithMany()
                        .HasForeignKey("IdJogo")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("APIFCG.Infra.Entity.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Jogo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("APIFCG.Infra.Entity.PromocaoJogo", b =>
                {
                    b.HasOne("APIFCG.Infra.Entity.Jogo", "Jogo")
                        .WithMany()
                        .HasForeignKey("IdJogo")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Jogo");
                });
#pragma warning restore 612, 618
        }
    }
}
