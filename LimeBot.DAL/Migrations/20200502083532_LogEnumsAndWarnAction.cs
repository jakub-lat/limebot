using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
{
    public partial class LogEnumsAndWarnAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WarnActionEnabled",
                table: "Guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WarnActionTreshold",
                table: "Guilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarnActionEnabled",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "WarnActionTreshold",
                table: "Guilds");
        }
    }
}
