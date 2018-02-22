using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Djm.OGame.Web.Api.Dal.Data.Migrations
{
    public partial class PinUniqueIndexMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pins_UniverseId_OwnerId_TargetId",
                table: "Pins",
                columns: new[] { "UniverseId", "OwnerId", "TargetId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pins_UniverseId_OwnerId_TargetId",
                table: "Pins");
        }
    }
}
