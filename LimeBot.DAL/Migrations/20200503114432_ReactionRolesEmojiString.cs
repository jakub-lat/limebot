using Microsoft.EntityFrameworkCore.Migrations;

namespace LimeBot.DAL.Migrations
{
    public partial class ReactionRolesEmojiString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmojiId",
                table: "ReactionRoles");

            migrationBuilder.DropColumn(
                name: "EmojiIsAnimated",
                table: "ReactionRoles");

            migrationBuilder.DropColumn(
                name: "EmojiName",
                table: "ReactionRoles");

            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "ReactionRoles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "ReactionRoles");

            migrationBuilder.AddColumn<decimal>(
                name: "EmojiId",
                table: "ReactionRoles",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "EmojiIsAnimated",
                table: "ReactionRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmojiName",
                table: "ReactionRoles",
                type: "text",
                nullable: true);
        }
    }
}
