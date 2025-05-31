using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIFCG.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Jogo",
                columns: table => new
                {
                    idJogo = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeAbreviado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataLancamento = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ValorVenda = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    UsuarioResponsavelCadastro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogo", x => x.idJogo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lvl = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PromocaoJogo",
                columns: table => new
                {
                    IdJogo = table.Column<int>(type: "INT", nullable: false),
                    DataInicioPromocao = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DataFimPromocao = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ValorJogoPromocao = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocaoJogo", x => x.IdJogo);
                    table.ForeignKey(
                        name: "FK_PromocaoJogo_Jogo_IdJogo",
                        column: x => x.IdJogo,
                        principalTable: "Jogo",
                        principalColumn: "idJogo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BibliotecaJogosCliente",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INT", nullable: false),
                    IdJogo = table.Column<int>(type: "INT", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ValorCompra = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibliotecaJogosCliente", x => new { x.IdUsuario, x.IdJogo });
                    table.ForeignKey(
                        name: "FK_BibliotecaJogosCliente_Jogo_IdJogo",
                        column: x => x.IdJogo,
                        principalTable: "Jogo",
                        principalColumn: "idJogo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibliotecaJogosCliente_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BibliotecaJogosCliente_IdJogo",
                table: "BibliotecaJogosCliente",
                column: "IdJogo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibliotecaJogosCliente");

            migrationBuilder.DropTable(
                name: "PromocaoJogo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Jogo");
        }
    }
}
