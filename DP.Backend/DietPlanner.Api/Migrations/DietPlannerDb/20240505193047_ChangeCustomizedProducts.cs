using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations.DietPlannerDb
{
    public partial class ChangeCustomizedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomizedPortionMultiplier",
                table: "DishProducts");

            migrationBuilder.CreateTable(
                name: "CustomizedDishProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishProductId = table.Column<int>(type: "int", nullable: false),
                    MealDishId = table.Column<int>(type: "int", nullable: false),
                    CustomizedPortionMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 1m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedDishProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizedDishProducts_DishProducts_DishProductId",
                        column: x => x.DishProductId,
                        principalTable: "DishProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomizedDishProducts_MealDishes_MealDishId",
                        column: x => x.MealDishId,
                        principalTable: "MealDishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedDishProducts_DishProductId",
                table: "CustomizedDishProducts",
                column: "DishProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedDishProducts_MealDishId",
                table: "CustomizedDishProducts",
                column: "MealDishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizedDishProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "CustomizedPortionMultiplier",
                table: "DishProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 1m);
        }
    }
}
