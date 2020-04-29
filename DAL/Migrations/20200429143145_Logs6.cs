using Microsoft.EntityFrameworkCore.Migrations;
using PotatoBot.Models;

namespace DAL.Migrations
{
    public partial class Logs6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<GuildLog[]>(
                name: "Logs",
                table: "Guilds",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logs",
                table: "Guilds");
        }
    }
}
