using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class PlayerTableAddMailMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowNotifications",
                table: "Players",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowNotifications",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Players");
        }
    }
}
