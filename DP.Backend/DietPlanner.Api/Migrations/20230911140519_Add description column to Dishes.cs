using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class AdddescriptioncolumntoDishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Dishes",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Dishes");
        }
    }
}
