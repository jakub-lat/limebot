using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class LogsList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildLog_Guilds_GuildDataId",
                table: "GuildLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildLog",
                table: "GuildLog");

            migrationBuilder.RenameTable(
                name: "GuildLog",
                newName: "Logs");

            migrationBuilder.RenameIndex(
                name: "IX_GuildLog_GuildDataId",
                table: "Logs",
                newName: "IX_Logs_GuildDataId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Logs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs",
                column: "GuildDataId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Guilds_GuildDataId",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "GuildLog");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_GuildDataId",
                table: "GuildLog",
                newName: "IX_GuildLog_GuildDataId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GuildLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildLog",
                table: "GuildLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildLog_Guilds_GuildDataId",
                table: "GuildLog",
                column: "GuildDataId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
