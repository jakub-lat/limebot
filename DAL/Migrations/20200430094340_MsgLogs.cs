using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class MsgLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableMessageLogs",
                table: "Guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MessageLogsChannel",
                table: "Guilds",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableMessageLogs",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "MessageLogsChannel",
                table: "Guilds");
        }
    }
}
