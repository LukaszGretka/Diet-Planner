using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class AdddescriptioncolumntoDishes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DishProducts_Id",
                table: "DishProducts",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishProducts_Id",
                table: "DishProducts");
        }
    }
}
