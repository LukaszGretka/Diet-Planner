using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class dbstructureimprovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_MealTypes_MealTypeId",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_Id",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_MealTypeId",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_MealProducts_Id",
                table: "MealProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MealProducts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "MealProducts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTypes_Meals_Id",
                table: "MealTypes",
                column: "Id",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTypes_Meals_Id",
                table: "MealTypes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MealProducts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "MealProducts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_Id",
                table: "Meals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MealTypeId",
                table: "Meals",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealProducts_Id",
                table: "MealProducts",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_MealTypes_MealTypeId",
                table: "Meals",
                column: "MealTypeId",
                principalTable: "MealTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
