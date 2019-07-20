using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class CorreçãoDataCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegister",
                table: "Users",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "(NOW())",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "(NOW())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegister",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValueSql: "(NOW())",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "(NOW())");
        }
    }
}
