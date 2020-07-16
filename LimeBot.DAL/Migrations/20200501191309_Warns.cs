using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
{
    public partial class Warns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<ulong, int>>(
                name: "Warns",
                table: "Guilds",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Warns",
                table: "Guilds");
        }
    }
}
