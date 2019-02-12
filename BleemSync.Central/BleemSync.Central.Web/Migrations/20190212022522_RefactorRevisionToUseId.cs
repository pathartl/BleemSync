using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Central.Web.Migrations
{
    public partial class RefactorRevisionToUseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RevisedGameId",
                table: "PlayStation_GameRevisions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevisedGameId",
                table: "PlayStation_GameRevisions");
        }
    }
}
