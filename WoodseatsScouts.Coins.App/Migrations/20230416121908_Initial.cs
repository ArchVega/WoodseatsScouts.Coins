using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoodseatsScouts.Coins.App.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TroopNumber = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue1State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue2State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clue3State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoutPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutId = table.Column<int>(type: "int", nullable: false),
                    ScannedCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseNumber = table.Column<int>(type: "int", nullable: false),
                    PointValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoutPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoutPoints_Scouts_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "Scouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Scouts",
                columns: new[] { "Id", "Clue1State", "Clue2State", "Clue3State", "Name", "ScoutNumber", "Section", "TroopNumber" },
                values: new object[,]
                {
                    { 1, null, null, null, "Scout A", 4, "B", 13 },
                    { 2, null, null, null, "Scout B", 5, "B", 13 },
                    { 3, null, null, null, "Scout C", 8, "B", 13 },
                    { 4, null, null, null, "Scout D", 10, "C", 16 },
                    { 5, null, null, null, "Scout E", 19, "C", 16 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoutPoints_ScoutId",
                table: "ScoutPoints",
                column: "ScoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoutPoints");

            migrationBuilder.DropTable(
                name: "Scouts");
        }
    }
}
