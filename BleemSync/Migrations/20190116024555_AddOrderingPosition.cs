using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Migrations
{
    public partial class AddOrderingPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "GameManagerNodes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "GameManagerNodes");
        }
    }
}
