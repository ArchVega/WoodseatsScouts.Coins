using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoodseatsScouts.Coins.Api.Migrations
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    Base = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true)
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
                table: "Coins",
                columns: new[] { "Id", "Base", "Code", "MemberId", "Value" },
                values: new object[,]
                {
                    { 1, 2, "B0002002003", null, 3 },
                    { 2, 3, "B0003003009", null, 9 },
                    { 3, 1, "B0004001010", null, 10 },
                    { 4, 2, "B0005002011", null, 11 },
                    { 5, 3, "B0006003020", null, 20 },
                    { 6, 1, "B0007001003", null, 3 },
                    { 7, 2, "B0008002009", null, 9 },
                    { 8, 3, "B0009003010", null, 10 },
                    { 9, 1, "B0010001011", null, 11 },
                    { 10, 2, "B0011002020", null, 20 },
                    { 11, 3, "B0012003003", null, 3 },
                    { 12, 1, "B0013001009", null, 9 },
                    { 13, 2, "B0014002010", null, 10 },
                    { 14, 3, "B0015003011", null, 11 },
                    { 15, 1, "B0016001020", null, 20 },
                    { 16, 2, "B0017002003", null, 3 },
                    { 17, 3, "B0018003009", null, 9 },
                    { 18, 1, "B0019001010", null, 10 },
                    { 19, 2, "B0020002011", null, 11 },
                    { 20, 3, "B0021003020", null, 20 },
                    { 21, 1, "B0022001003", null, 3 },
                    { 22, 2, "B0023002009", null, 9 },
                    { 23, 3, "B0024003010", null, 10 },
                    { 24, 1, "B0025001011", null, 11 },
                    { 25, 2, "B0026002020", null, 20 },
                    { 26, 3, "B0027003003", null, 3 },
                    { 27, 1, "B0028001009", null, 9 },
                    { 28, 2, "B0029002010", null, 10 },
                    { 29, 3, "B0030003011", null, 11 },
                    { 30, 1, "B0031001020", null, 20 },
                    { 31, 2, "B0032002003", null, 3 },
                    { 32, 3, "B0033003009", null, 9 },
                    { 33, 1, "B0034001010", null, 10 },
                    { 34, 2, "B0035002011", null, 11 },
                    { 35, 3, "B0036003020", null, 20 },
                    { 36, 1, "B0037001003", null, 3 },
                    { 37, 2, "B0038002009", null, 9 },
                    { 38, 3, "B0039003010", null, 10 },
                    { 39, 1, "B0040001011", null, 11 },
                    { 40, 2, "B0041002020", null, 20 },
                    { 41, 3, "B0042003003", null, 3 },
                    { 42, 1, "B0043001009", null, 9 }
                });

            migrationBuilder.InsertData(
                table: "Coins",
                columns: new[] { "Id", "Base", "Code", "MemberId", "Value" },
                values: new object[,]
                {
                    { 43, 2, "B0044002010", null, 10 },
                    { 44, 3, "B0045003011", null, 11 },
                    { 45, 1, "B0046001020", null, 20 },
                    { 46, 2, "B0047002003", null, 3 },
                    { 47, 3, "B0048003009", null, 9 },
                    { 48, 1, "B0049001010", null, 10 },
                    { 49, 2, "B0050002011", null, 11 },
                    { 50, 3, "B0051003020", null, 20 }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "A", "Adults" },
                    { "B", "Beavers" },
                    { "C", "Cubs" },
                    { "E", "Explorers" },
                    { "S", "Scouts" }
                });

            migrationBuilder.InsertData(
                table: "Troops",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Charcoal" },
                    { 2, "Jet" },
                    { 3, "Hunter" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Clue1State", "Clue2State", "Clue3State", "FirstName", "HasImage", "IsDayVisitor", "LastName", "Number", "SectionId", "TroopId" },
                values: new object[] { 1, null, null, null, "Crimson", true, false, "Charcoal", 0, "A", 1 });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Clue1State", "Clue2State", "Clue3State", "FirstName", "HasImage", "IsDayVisitor", "LastName", "Number", "SectionId", "TroopId" },
                values: new object[] { 5, null, null, null, "Glaucous", true, false, "Jet", 0, "B", 2 });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Clue1State", "Clue2State", "Clue3State", "FirstName", "HasImage", "IsDayVisitor", "LastName", "Number", "SectionId", "TroopId" },
                values: new object[] { 13, null, null, null, "Saffron", true, false, "Hunter", 0, "C", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Coins_MemberId",
                table: "Coins",
                column: "MemberId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "ScavengedCoins");

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
