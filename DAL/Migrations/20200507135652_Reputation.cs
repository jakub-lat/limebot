using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class Reputation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableReputation",
                table: "Guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReputationXP",
                table: "Guilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableReputation",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "ReputationXP",
                table: "Guilds");
        }
    }
}
