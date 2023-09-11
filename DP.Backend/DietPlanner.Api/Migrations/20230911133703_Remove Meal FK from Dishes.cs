using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class RemoveMealFKfromDishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Meals_MealId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Dishes_DishesId",
                table: "MealDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Meals_MealsId",
                table: "MealDishes");

            migrationBuilder.DropIndex(
                name: "IX_Meals_Id",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_MealDishes_DishesId",
                table: "MealDishes");

            migrationBuilder.DropIndex(
                name: "IX_MealDishes_MealsId",
                table: "MealDishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_MealId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "DishesId",
                table: "MealDishes");

            migrationBuilder.DropColumn(
                name: "MealsId",
                table: "MealDishes");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "Dishes");

            migrationBuilder.AddColumn<int>(
                name: "DishId",
                table: "MealDishes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MealId",
                table: "MealDishes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_DishId",
                table: "MealDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_MealId",
                table: "MealDishes",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Dishes_DishId",
                table: "MealDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Meals_MealId",
                table: "MealDishes",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Dishes_DishId",
                table: "MealDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Meals_MealId",
                table: "MealDishes");

            migrationBuilder.DropIndex(
                name: "IX_MealDishes_DishId",
                table: "MealDishes");

            migrationBuilder.DropIndex(
                name: "IX_MealDishes_MealId",
                table: "MealDishes");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "MealDishes");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "MealDishes");

            migrationBuilder.AddColumn<int>(
                name: "DishesId",
                table: "MealDishes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MealsId",
                table: "MealDishes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MealId",
                table: "Dishes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_Id",
                table: "Meals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_DishesId",
                table: "MealDishes",
                column: "DishesId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_MealsId",
                table: "MealDishes",
                column: "MealsId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MealId",
                table: "Dishes",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Meals_MealId",
                table: "Dishes",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Dishes_DishesId",
                table: "MealDishes",
                column: "DishesId",
                principalTable: "Dishes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Meals_MealsId",
                table: "MealDishes",
                column: "MealsId",
                principalTable: "Meals",
                principalColumn: "Id");
        }
    }
}
