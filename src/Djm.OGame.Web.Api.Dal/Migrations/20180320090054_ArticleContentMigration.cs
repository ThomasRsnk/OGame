using Microsoft.EntityFrameworkCore.Migrations;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    public partial class ArticleContentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.RenameColumn(
                name: "HtmlContentId",
                table: "Articles",
                newName: "ContentId");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "ArticlesContents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE ArticlesContents
                   SET ArticleId = Id;  
            ");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ArticlesContents_ArticleId",
                table: "ArticlesContents",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ContentId",
                table: "Articles",
                column: "ContentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticlesContents_ContentId",
                table: "Articles",
                column: "ContentId",
                principalTable: "ArticlesContents",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticlesContents_ContentId",
                table: "Articles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ArticlesContents_ArticleId",
                table: "ArticlesContents");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ContentId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "ArticlesContents");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Articles",
                newName: "HtmlContentId");
        }
    }
}
