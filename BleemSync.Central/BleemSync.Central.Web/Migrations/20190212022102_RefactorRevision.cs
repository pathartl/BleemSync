using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Central.Web.Migrations
{
    public partial class RefactorRevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRevisions");

            migrationBuilder.CreateTable(
                name: "PlayStation_GameRevisions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubmittedById = table.Column<string>(nullable: true),
                    SubmittedOn = table.Column<DateTime>(nullable: true),
                    ApprovedById = table.Column<string>(nullable: true),
                    ApprovedOn = table.Column<DateTime>(nullable: true),
                    RejectedById = table.Column<string>(nullable: true),
                    RejectedOn = table.Column<DateTime>(nullable: true),
                    GameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayStation_GameRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayStation_GameRevisions_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayStation_GameRevisions_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayStation_GameRevisions_AspNetUsers_RejectedById",
                        column: x => x.RejectedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayStation_GameRevisions_AspNetUsers_SubmittedById",
                        column: x => x.SubmittedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_GameRevisions_ApprovedById",
                table: "PlayStation_GameRevisions",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_GameRevisions_GameId",
                table: "PlayStation_GameRevisions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_GameRevisions_RejectedById",
                table: "PlayStation_GameRevisions",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStation_GameRevisions_SubmittedById",
                table: "PlayStation_GameRevisions",
                column: "SubmittedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayStation_GameRevisions");

            migrationBuilder.CreateTable(
                name: "GameRevisions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApprovedById = table.Column<string>(nullable: true),
                    ApprovedOn = table.Column<DateTime>(nullable: true),
                    GameId = table.Column<int>(nullable: true),
                    RejectedById = table.Column<string>(nullable: true),
                    RejectedOn = table.Column<DateTime>(nullable: true),
                    SubmittedById = table.Column<string>(nullable: true),
                    SubmittedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRevisions_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameRevisions_PlayStation_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "PlayStation_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameRevisions_AspNetUsers_RejectedById",
                        column: x => x.RejectedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameRevisions_AspNetUsers_SubmittedById",
                        column: x => x.SubmittedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRevisions_ApprovedById",
                table: "GameRevisions",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_GameRevisions_GameId",
                table: "GameRevisions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRevisions_RejectedById",
                table: "GameRevisions",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_GameRevisions_SubmittedById",
                table: "GameRevisions",
                column: "SubmittedById");
        }
    }
}
