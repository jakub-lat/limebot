using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Members",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
