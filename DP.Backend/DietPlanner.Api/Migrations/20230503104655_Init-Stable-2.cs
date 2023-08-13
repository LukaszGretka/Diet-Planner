using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class InitStable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTypes_Meals_Id",
                table: "MealTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meals",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Meals");

            migrationBuilder.RenameTable(
                name: "Meals",
                newName: "UserMeals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMeals",
                table: "UserMeals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_UserMeals_MealId",
                table: "MealProducts",
                column: "MealId",
                principalTable: "UserMeals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTypes_UserMeals_Id",
                table: "MealTypes",
                column: "Id",
                principalTable: "UserMeals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_UserMeals_MealId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTypes_UserMeals_Id",
                table: "MealTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMeals",
                table: "UserMeals");

            migrationBuilder.RenameTable(
                name: "UserMeals",
                newName: "Meals");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Meals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meals",
                table: "Meals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTypes_Meals_Id",
                table: "MealTypes",
                column: "Id",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
