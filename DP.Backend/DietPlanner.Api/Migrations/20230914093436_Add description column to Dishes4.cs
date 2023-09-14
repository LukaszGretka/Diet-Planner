using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class AdddescriptioncolumntoDishes4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExposedForOtherUsers",
                table: "Dishes",
                newName: "ExposeToOtherUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExposeToOtherUsers",
                table: "Dishes",
                newName: "ExposedForOtherUsers");
        }
    }
}
