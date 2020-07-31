using Microsoft.EntityFrameworkCore.Migrations;

namespace CasaDoCodigo.MVC.Migrations.SqlServerMigrations
{
    public partial class Pedido_ClienteId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClienteId",
                table: "Pedido",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Cadastro",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Pedido");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Cadastro",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
