using Microsoft.EntityFrameworkCore.Migrations;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class PlayerAddSaltMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Players");
        }
    }
}
