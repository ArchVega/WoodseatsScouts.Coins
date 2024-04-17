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
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
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
                name: "Sections",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Code);
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "'M' + (FORMAT(TroopId, '000'))  + [SectionId] + (FORMAT(Number, '000'))"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TroopId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<string>(type: "char(1)", nullable: false),
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
                        name: "FK_Members_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Troops_TroopId",
                        column: x => x.TroopId,
                        principalTable: "Troops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseValueId = table.Column<int>(type: "int", nullable: false),
                    Base = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false, computedColumnSql: "'C' + (FORMAT([BaseValueId], '0000'))  + (FORMAT([Base], '000')) + (FORMAT([Value], '000'))"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    LockUntil = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coins_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberCountryVotes",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCountryVotes", x => new { x.MemberId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_MemberCountryVotes_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberCountryVotes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
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
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Italy" },
                    { 2, "Germany" },
                    { 3, "France" },
                    { 4, "Belgium" },
                    { 5, "Ireland" },
                    { 6, "Poland" },
                    { 7, "Australia" },
                    { 8, "Finland" },
                    { 9, "Norway" },
                    { 10, "Spain" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coins_MemberId",
                table: "Coins",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCountryVotes_CountryId",
                table: "MemberCountryVotes",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_SectionId",
                table: "Members",
                column: "SectionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Code",
                table: "Sections",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "MemberCountryVotes");

            migrationBuilder.DropTable(
                name: "ScavengedCoins");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ScavengeResults");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Troops");
        }
    }
}
