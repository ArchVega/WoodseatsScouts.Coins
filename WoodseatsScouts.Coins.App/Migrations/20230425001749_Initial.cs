using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoodseatsScouts.Coins.App.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Troops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Troops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "'M' + (FORMAT(TroopId, '000'))  + [Section] + (FORMAT(Number, '000'))"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TroopId = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clue1State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue2State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue3State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDayVisitor = table.Column<bool>(type: "bit", nullable: false),
                    HasImage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Troops_TroopId",
                        column: x => x.TroopId,
                        principalTable: "Troops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScavengeResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScavengeResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScavengeResults_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScavengedCoins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScavengeResultId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseNumber = table.Column<int>(type: "int", nullable: false),
                    PointValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScavengedCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScavengedCoins_ScavengeResults_ScavengeResultId",
                        column: x => x.ScavengeResultId,
                        principalTable: "ScavengeResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Troops",
                columns: new[] { "Id", "Name" },
                values: new object[] { 74, "Oak Street" });

            migrationBuilder.InsertData(
                table: "Troops",
                columns: new[] { "Id", "Name" },
                values: new object[] { 280, "Norton" });

            migrationBuilder.InsertData(
                table: "Troops",
                columns: new[] { "Id", "Name" },
                values: new object[] { 999, "Woodseats Explorers" });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Clue1State", "Clue2State", "Clue3State", "FirstName", "HasImage", "IsDayVisitor", "LastName", "Number", "Section", "TroopId" },
                values: new object[,]
                {
                    { 1, null, null, null, "Conner", true, false, "Gillespie", 1, "B", 999 },
                    { 2, null, null, null, "Orlando", true, false, "Mendez", 2, "B", 999 },
                    { 3, null, null, null, "Calvin", true, false, "Fields", 3, "B", 999 },
                    { 4, null, null, null, "Dillon", true, true, "Durham", 4, "C", 74 },
                    { 5, null, null, null, "Josiah", true, true, "Castaneda", 5, "C", 74 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_TroopId",
                table: "Members",
                column: "TroopId");

            migrationBuilder.CreateIndex(
                name: "IX_ScavengedCoins_ScavengeResultId",
                table: "ScavengedCoins",
                column: "ScavengeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ScavengeResults_MemberId",
                table: "ScavengeResults",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "ScavengedCoins");

            migrationBuilder.DropTable(
                name: "ScavengeResults");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Troops");
        }
    }
}
