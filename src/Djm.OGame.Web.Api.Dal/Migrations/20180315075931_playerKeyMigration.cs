using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class playerKeyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "ArticlesContents");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEmail",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorEmail",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "ArticlesContents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Articles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UniverseId = table.Column<int>(nullable: false),
                    AllowNotifications = table.Column<bool>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ProfilePicturePath = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => new { x.Id, x.UniverseId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_UniverseId_Id_Login",
                table: "Players",
                columns: new[] { "UniverseId", "Id", "Login" },
                unique: true,
                filter: "[Login] IS NOT NULL");
        }
    }
}
