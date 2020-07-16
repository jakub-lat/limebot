using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LimeBot.DAL.Migrations
{
    public partial class ReactionRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReactionRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Emote = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionRoles");
        }
    }
}
