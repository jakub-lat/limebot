using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
{
    public partial class LevelUpMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableLevelUpMessage",
                table: "Guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LevelUpMessage",
                table: "Guilds",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableLevelUpMessage",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "LevelUpMessage",
                table: "Guilds");
        }
    }
}
