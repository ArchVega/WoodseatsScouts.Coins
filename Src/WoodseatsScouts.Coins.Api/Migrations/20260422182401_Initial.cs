using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WoodseatsScouts.Coins.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityBases", x => x.Id);
                });

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
                name: "ScoutGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoutGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoutSections",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoutSections", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ScoutMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "'M' + (FORMAT(ScoutGroupId, '000'))  + [ScoutSectionId] + (FORMAT(Number, '000'))"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoutGroupId = table.Column<int>(type: "int", nullable: false),
                    ScoutSectionId = table.Column<string>(type: "char(1)", nullable: false),
                    Clue1State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue2State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue3State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDayVisitor = table.Column<bool>(type: "bit", nullable: false),
                    HasImage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoutMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoutMembers_ScoutGroups_ScoutGroupId",
                        column: x => x.ScoutGroupId,
                        principalTable: "ScoutGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoutMembers_ScoutSections_ScoutSectionId",
                        column: x => x.ScoutSectionId,
                        principalTable: "ScoutSections",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityBaseSequenceNumber = table.Column<int>(type: "int", nullable: false),
                    ActivityBaseId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false, computedColumnSql: "'C' + (FORMAT([ActivityBaseSequenceNumber], '0000'))  + (FORMAT([ActivityBaseId], '000')) + (FORMAT([Value], '000'))"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    LockUntil = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coins_ActivityBases_ActivityBaseId",
                        column: x => x.ActivityBaseId,
                        principalTable: "ActivityBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coins_ScoutMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ScoutMembers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScanSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutMemberId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScanSessions_ScoutMembers_ScoutMemberId",
                        column: x => x.ScoutMemberId,
                        principalTable: "ScoutMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScanCoins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanSessionId = table.Column<int>(type: "int", nullable: false),
                    CoinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScanCoins_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScanCoins_ScanSessions_ScanSessionId",
                        column: x => x.ScanSessionId,
                        principalTable: "ScanSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActivityBases",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Archery" },
                    { 2, "Abseiling" },
                    { 3, "Aerial Trek" },
                    { 4, "Aeroball" },
                    { 5, "Bouldering" },
                    { 6, "Bushcraft" },
                    { 7, "Campfire" },
                    { 8, "Canoeing" },
                    { 9, "Caving" },
                    { 10, "Fencing" },
                    { 11, "Hike" },
                    { 12, "Hillwalking" },
                    { 13, "Kayaking" },
                    { 14, "Orienteering" },
                    { 15, "Pioneering" },
                    { 16, "Powerboating" },
                    { 17, "Raft Building" },
                    { 18, "Sailing" },
                    { 19, "Tomahawk throwing" },
                    { 20, "Zip wire" },
                    { 99, "Misc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coins_ActivityBaseId",
                table: "Coins",
                column: "ActivityBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Coins_MemberId",
                table: "Coins",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanCoins_CoinId",
                table: "ScanCoins",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanCoins_ScanSessionId",
                table: "ScanCoins",
                column: "ScanSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanSessions_ScoutMemberId",
                table: "ScanSessions",
                column: "ScoutMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoutMembers_ScoutGroupId",
                table: "ScoutMembers",
                column: "ScoutGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoutMembers_ScoutSectionId",
                table: "ScoutMembers",
                column: "ScoutSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoutSections_Code",
                table: "ScoutSections",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "ScanCoins");

            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "ScanSessions");

            migrationBuilder.DropTable(
                name: "ActivityBases");

            migrationBuilder.DropTable(
                name: "ScoutMembers");

            migrationBuilder.DropTable(
                name: "ScoutGroups");

            migrationBuilder.DropTable(
                name: "ScoutSections");
        }
    }
}
