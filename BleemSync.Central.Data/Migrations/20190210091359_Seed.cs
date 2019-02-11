using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Central.Data.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayStation_Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    MemoryCardBlockCount = table.Column<int>(nullable: false),
                    MultitapCompatible = table.Column<bool>(nullable: false),
                    LinkCableCompatible = table.Column<bool>(nullable: false),
                    VibrationCompatible = table.Column<bool>(nullable: false),
                    AnalogCompatible = table.Column<bool>(nullable: false),
                    DigitalCompatible = table.Column<bool>(nullable: false),
                    LightGunCompatible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayStation_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EsrbRatingDescriptors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsrbRatingDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EsrbRatingDescriptors_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PegiRatingDescriptors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PegiRatingDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PegiRatingDescriptors_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayStation_Discs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fingerprint = table.Column<string>(nullable: true),
                    DiscNumber = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: true),
                    TrackCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayStation_Discs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayStation_Discs_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayStation_Art",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    Rating = table.Column<decimal>(nullable: false),
                    RatingVoteCount = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsGreatestHits = table.Column<bool>(nullable: false),
                    DiscId = table.Column<int>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayStation_Art", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayStation_Art_PlayStation_Discs_DiscId",
                        column: x => x.DiscId,
                        principalTable: "PlayStation_Discs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayStation_Art_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EsrbRatingDescriptors_GameId",
                table: "EsrbRatingDescriptors",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_GameId",
                table: "Genres",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PegiRatingDescriptors_GameId",
                table: "PegiRatingDescriptors",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_Art_DiscId",
                table: "PlayStation_Art",
                column: "DiscId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_Art_GameId",
                table: "PlayStation_Art",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_Discs_GameId",
                table: "PlayStation_Discs",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EsrbRatingDescriptors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "PegiRatingDescriptors");

            migrationBuilder.DropTable(
                name: "PlayStation_Art");

            migrationBuilder.DropTable(
                name: "PlayStation_Discs");

            migrationBuilder.DropTable(
                name: "PlayStation_Games");
        }
    }
}
