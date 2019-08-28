using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class AddStatusUsu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Users",
                type: "BIT",
                nullable: false,
                defaultValueSql: "(1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");
        }
    }
}
