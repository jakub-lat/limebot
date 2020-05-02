using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PotatoBot.Models;

namespace PotatoBot.Migrations
{
    public partial class RemoveLogsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logs",
                table: "Guilds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<GuildLog>>(
                name: "Logs",
                table: "Guilds",
                type: "jsonb",
                nullable: true);
        }
    }
}
