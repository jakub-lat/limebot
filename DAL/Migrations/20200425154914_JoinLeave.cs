using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class JoinLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Prefix = table.Column<string>(nullable: true),
                    MutedRoleId = table.Column<ulong>(nullable: false),
                    AutoRolesOnJoin = table.Column<string>(nullable: true),
                    EnableWelcomeMessages = table.Column<bool>(nullable: false),
                    WelcomeMessagesChannel = table.Column<ulong>(nullable: false),
                    WelcomeMessage = table.Column<string>(nullable: true),
                    LeaveMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GuildDataId = table.Column<ulong>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    AuthorId = table.Column<ulong>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Guilds_GuildDataId",
                        column: x => x.GuildDataId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GuildDataId",
                table: "Logs",
                column: "GuildDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
