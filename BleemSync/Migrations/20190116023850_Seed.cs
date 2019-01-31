using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BleemSync.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite.Autoincrement", true),
                    Core_Name = table.Column<string>(nullable: false),
                    Core_FS_Location = table.Column<string>(nullable: false)
                },
                constraints:table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "GameSystemDefaultCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    System_ID = table.Column<int>(nullable: false),
                    Core_ID = table.Column<int>(nullable:false)
                },
                constraints:table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                    name:"FK_GameSystemDefaultCore_GameSystem_PK_GameSystem",
                    column: x => x.System_ID,
                    principalTable:"GameSystem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                    name:"FK_GameSystemDefaultCore_Core_PK_Cores",
                    column: x => x.Core_ID,
                    principalTable:"Cores",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);

                }
            );

            migrationBuilder.CreateTable(
                name: "GameManagerNodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    SortName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true),
                    Players = table.Column<int>(nullable: true),
                    Developer = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    SystemId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameManagerNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameManagerNodes_GameManagerNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "GameManagerNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameManagerNodes_GameSystem_SystemId",
                        column: x => x.SystemId,
                        principalTable: "GameSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    GameManagerNodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameGenre_GameManagerNodes_GameManagerNodeId",
                        column: x => x.GameManagerNodeId,
                        principalTable: "GameManagerNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameManagerFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    NodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameManagerFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameManagerFiles_GameManagerNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "GameManagerNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMeta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMeta_GameManagerNodes_GameId",
                        column: x => x.GameId,
                        principalTable: "GameManagerNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GameManagerNodeId",
                table: "GameGenre",
                column: "GameManagerNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameManagerFiles_NodeId",
                table: "GameManagerFiles",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameManagerNodes_ParentId",
                table: "GameManagerNodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GameManagerNodes_SystemId",
                table: "GameManagerNodes",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMeta_GameId",
                table: "GameMeta",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GameManagerFiles");

            migrationBuilder.DropTable(
                name: "GameMeta");

            migrationBuilder.DropTable(
                name: "GameManagerNodes");

            migrationBuilder.DropTable(
                name: "GameSystem");
        }
    }
}
