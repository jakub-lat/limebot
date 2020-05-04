using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class msgxp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxMessageXP",
                table: "Guilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinMessageXP",
                table: "Guilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxMessageXP",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "MinMessageXP",
                table: "Guilds");
        }
    }
}
