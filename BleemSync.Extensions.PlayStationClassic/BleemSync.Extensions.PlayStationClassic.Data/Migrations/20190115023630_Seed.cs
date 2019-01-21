using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Data.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MENU_ENTRIES",
                columns: table => new
                {
                    GAME_ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_TITLE_STRING = table.Column<string>(nullable: true),
                    PUBLISHER_NAME = table.Column<string>(nullable: true),
                    RELEASE_YEAR = table.Column<int>(nullable: false),
                    PLAYERS = table.Column<int>(nullable: false),
                    RATING_IMAGE = table.Column<string>(nullable: true),
                    GAME_MANUAL_QR_IMAGE = table.Column<string>(nullable: true),
                    LINK_GAME_ID = table.Column<string>(nullable: true),
                    POSITION = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAME", x => x.GAME_ID);
                });

            migrationBuilder.CreateTable(
                name: "LANGUAGE_SPECIFIC",
                columns: table => new
                {
                    LANGUAGE_ID = table.Column<string>(nullable: false),
                    DEFAULT_VALUE = table.Column<string>(nullable: true),
                    VALUE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LANGUAGE_SPECIFIC", x => x.LANGUAGE_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISC",
                columns: table => new
                {
                    DISC_ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(nullable: false),
                    DISC_NUMBER = table.Column<int>(nullable: false),
                    BASENAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISC", x => x.DISC_ID);
                    table.ForeignKey(
                        name: "FK_DISC_GAME_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "MENU_ENTRIES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DISC_GAME_ID",
                table: "DISC",
                column: "GAME_ID");

            migrationBuilder.Sql("CREATE VIEW GAME AS SELECT * FROM MENU_ENTRIES ORDER BY POSITION, GAME_TITLE_STRING;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DISC");

            migrationBuilder.DropTable(
                name: "LANGUAGE_SPECIFIC");

            migrationBuilder.DropTable(
                name: "MENU_ENTRIES");

            migrationBuilder.Sql("DROP VIEW GAMES;");
        }
    }
}
