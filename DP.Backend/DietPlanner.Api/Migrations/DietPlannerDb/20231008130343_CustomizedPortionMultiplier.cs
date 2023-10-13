using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations.DietPlannerDb
{
    public partial class CustomizedPortionMultiplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CustomizedPortionMultiplier",
                table: "DishProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 1m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomizedPortionMultiplier",
                table: "DishProducts");
        }
    }
}
