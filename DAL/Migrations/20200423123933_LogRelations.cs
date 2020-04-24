using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class LogRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "GuildDataId",
                table: "Logs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs",
                column: "GuildDataId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "GuildDataId",
                table: "Logs",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs",
                column: "GuildDataId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
