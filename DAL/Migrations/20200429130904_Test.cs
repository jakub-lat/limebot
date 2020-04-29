using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PotatoBot.Models;

namespace DAL.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    Logs = table.Column<List<GuildLog>>(type: "jsonb", nullable: true),
                    MutedRoleId = table.Column<decimal>(nullable: false),
                    AutoRolesOnJoin = table.Column<List<ulong>>(nullable: true),
                    EnableWelcomeMessages = table.Column<bool>(nullable: false),
                    WelcomeMessagesChannel = table.Column<decimal>(nullable: false),
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
                    GuildDataId = table.Column<decimal>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    AuthorId = table.Column<decimal>(nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
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
