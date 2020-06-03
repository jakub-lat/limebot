using Microsoft.EntityFrameworkCore.Migrations;

namespace PotatoBot.Migrations
{
    public partial class RolesForLevelAsJsonc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolesForLevel_Guilds_GuildId",
                table: "RolesForLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesForLevel",
                table: "RolesForLevel");

            migrationBuilder.RenameTable(
                name: "RolesForLevel",
                newName: "RoleForLevel");

            migrationBuilder.RenameIndex(
                name: "IX_RolesForLevel_GuildId",
                table: "RoleForLevel",
                newName: "IX_RoleForLevel_GuildId");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "RoleForLevel",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleForLevel",
                table: "RoleForLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleForLevel_Guilds_GuildId",
                table: "RoleForLevel",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleForLevel_Guilds_GuildId",
                table: "RoleForLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleForLevel",
                table: "RoleForLevel");

            migrationBuilder.RenameTable(
                name: "RoleForLevel",
                newName: "RolesForLevel");

            migrationBuilder.RenameIndex(
                name: "IX_RoleForLevel_GuildId",
                table: "RolesForLevel",
                newName: "IX_RolesForLevel_GuildId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Level",
                table: "RolesForLevel",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesForLevel",
                table: "RolesForLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolesForLevel_Guilds_GuildId",
                table: "RolesForLevel",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
