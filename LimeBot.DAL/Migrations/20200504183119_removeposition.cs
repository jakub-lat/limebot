using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
{
    public partial class removeposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Members",
                type: "integer",
                nullable: true);
        }
    }
}
