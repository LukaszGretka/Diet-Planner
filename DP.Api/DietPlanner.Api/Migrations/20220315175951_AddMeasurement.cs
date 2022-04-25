using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DietPlanner.Api.Migrations
{
    public partial class AddMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false),
                    Chest = table.Column<decimal>(type: "TEXT", nullable: false),
                    Belly = table.Column<decimal>(type: "TEXT", nullable: false),
                    Waist = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ForearmRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    ForearmLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfLeft = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_Id",
                table: "Measurements",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
