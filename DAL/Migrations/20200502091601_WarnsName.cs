using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class WarnsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warn_Guilds_GuildId",
                table: "Warn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warn",
                table: "Warn");

            migrationBuilder.RenameTable(
                name: "Warn",
                newName: "Warns");

            migrationBuilder.RenameIndex(
                name: "IX_Warn_GuildId",
                table: "Warns",
                newName: "IX_Warns_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warns",
                table: "Warns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warns_Guilds_GuildId",
                table: "Warns",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warns_Guilds_GuildId",
                table: "Warns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warns",
                table: "Warns");

            migrationBuilder.RenameTable(
                name: "Warns",
                newName: "Warn");

            migrationBuilder.RenameIndex(
                name: "IX_Warns_GuildId",
                table: "Warn",
                newName: "IX_Warn_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warn",
                table: "Warn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warn_Guilds_GuildId",
                table: "Warn",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
