using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class PlayerAccountingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");
        }
    }
}
