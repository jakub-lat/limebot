using System.Collections.Generic;
using LimeBot.DAL.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
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
