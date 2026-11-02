using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DenoXfs.Sim.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Track1 = table.Column<string>(type: "TEXT", nullable: true),
                    Track2 = table.Column<string>(type: "TEXT", nullable: true),
                    Track3 = table.Column<string>(type: "TEXT", nullable: true),
                    Atr = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceClass = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terminals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address1 = table.Column<string>(type: "TEXT", nullable: false),
                    Address2 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardChipResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Command = table.Column<string>(type: "TEXT", nullable: false),
                    Response = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardChipResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardChipResponses_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TerminalId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceClass = table.Column<string>(type: "TEXT", nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStates_Terminals_TerminalId",
                        column: x => x.TerminalId,
                        principalTable: "Terminals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardChipResponses_CardId_Command",
                table: "CardChipResponses",
                columns: new[] { "CardId", "Command" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Name",
                table: "Cards",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceData_DeviceClass_Category",
                table: "DeviceData",
                columns: new[] { "DeviceClass", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStates_TerminalId_DeviceClass",
                table: "DeviceStates",
                columns: new[] { "TerminalId", "DeviceClass" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_Address1",
                table: "Terminals",
                column: "Address1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardChipResponses");

            migrationBuilder.DropTable(
                name: "DeviceData");

            migrationBuilder.DropTable(
                name: "DeviceStates");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Terminals");
        }
    }
}
