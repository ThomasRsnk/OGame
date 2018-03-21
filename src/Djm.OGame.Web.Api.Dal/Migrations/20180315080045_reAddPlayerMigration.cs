using Microsoft.EntityFrameworkCore.Migrations;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class reAddPlayerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    EmailAddress = table.Column<string>(nullable: false),
                    AllowNotifications = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OGameId = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    ProfilePicturePath = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true),
                    UniverseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.EmailAddress);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_UniverseId_OGameId",
                table: "Players",
                columns: new[] { "UniverseId", "OGameId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
