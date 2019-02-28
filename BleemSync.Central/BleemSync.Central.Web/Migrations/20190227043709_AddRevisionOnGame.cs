using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Central.Web.Migrations
{
    public partial class AddRevisionOnGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RevisionId",
                table: "PlayStation_Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_GameRevisions_RevisedGameId",
                table: "PlayStation_GameRevisions",
                column: "RevisedGameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayStation_GameRevisions_PlayStation_Games_RevisedGameId",
                table: "PlayStation_GameRevisions",
                column: "RevisedGameId",
                principalTable: "PlayStation_Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayStation_GameRevisions_PlayStation_Games_RevisedGameId",
                table: "PlayStation_GameRevisions");

            migrationBuilder.DropIndex(
                name: "IX_PlayStation_GameRevisions_RevisedGameId",
                table: "PlayStation_GameRevisions");

            migrationBuilder.DropColumn(
                name: "RevisionId",
                table: "PlayStation_Games");
        }
    }
}
