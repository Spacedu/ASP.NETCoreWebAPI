using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MimicAPI.Migrations
{
    public partial class BancoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Palavras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    Pontuacao = table.Column<int>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    Criado = table.Column<DateTime>(nullable: false),
                    Atualizado = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palavras", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Palavras");
        }
    }
}
