using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PotatoBot.Migrations
{
    public partial class RolesForLevelAsJsonb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleForLevel");

            migrationBuilder.AddColumn<List<RoleForLevel>>(
                name: "RolesForLevel",
                table: "Guilds",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolesForLevel",
                table: "Guilds");

            migrationBuilder.CreateTable(
                name: "RoleForLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleForLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleForLevel_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleForLevel_GuildId",
                table: "RoleForLevel",
                column: "GuildId");
        }
    }
}
