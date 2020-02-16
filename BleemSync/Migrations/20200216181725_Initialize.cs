using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ShortTitle = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Region = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Fingerprint = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Developer = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    DateReleased = table.Column<DateTime>(nullable: false),
                    Region = table.Column<int>(nullable: false),
                    Players = table.Column<string>(nullable: true),
                    EsrbRating = table.Column<int>(nullable: false),
                    PegiRating = table.Column<int>(nullable: false),
                    OfficiallyLicensed = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    PlatformId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ProfileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collection_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionGame",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionGame", x => new { x.CollectionId, x.GameId });
                    table.ForeignKey(
                        name: "FK_CollectionGame_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionGame_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collection_ProfileId",
                table: "Collection",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionGame_GameId",
                table: "CollectionGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlatformId",
                table: "Games",
                column: "PlatformId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionGame");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
