using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class AddCustomizedMealProductsrenamecustomizeddishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizedDishProducts");

            migrationBuilder.CreateTable(
                name: "CustomizedMealDishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DishProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    MealDishId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomizedPortionMultiplier = table.Column<decimal>(type: "TEXT", precision: 6, scale: 2, nullable: false, defaultValue: 1m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedMealDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizedMealDishes_DishProducts_DishProductId",
                        column: x => x.DishProductId,
                        principalTable: "DishProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomizedMealDishes_MealDishes_MealDishId",
                        column: x => x.MealDishId,
                        principalTable: "MealDishes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomizedMealProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomizedPortionMultiplier = table.Column<decimal>(type: "TEXT", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedMealProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizedMealProducts_MealProducts_MealProductId",
                        column: x => x.MealProductId,
                        principalTable: "MealProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMealDishes_DishProductId",
                table: "CustomizedMealDishes",
                column: "DishProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMealDishes_MealDishId",
                table: "CustomizedMealDishes",
                column: "MealDishId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMealProducts_MealProductId",
                table: "CustomizedMealProducts",
                column: "MealProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizedMealDishes");

            migrationBuilder.DropTable(
                name: "CustomizedMealProducts");

            migrationBuilder.CreateTable(
                name: "CustomizedDishProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DishProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    MealDishId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomizedPortionMultiplier = table.Column<decimal>(type: "TEXT", precision: 6, scale: 2, nullable: false, defaultValue: 1m)
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
                        principalColumn: "Id");
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
    }
}
