using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasTarefasAPI.Migrations
{
    public partial class TarefaSincronizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tarefas",
                newName: "IdTarefaApi");

            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Tarefas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdTarefaApp",
                table: "Tarefas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "IdTarefaApp",
                table: "Tarefas");

            migrationBuilder.RenameColumn(
                name: "IdTarefaApi",
                table: "Tarefas",
                newName: "Id");
        }
    }
}
