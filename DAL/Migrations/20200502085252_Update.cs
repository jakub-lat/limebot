using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarnActionEnabled",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "WarnActionTreshold",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "WarnActionType",
                table: "Guilds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "WarnActionType",
                table: "Guilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
