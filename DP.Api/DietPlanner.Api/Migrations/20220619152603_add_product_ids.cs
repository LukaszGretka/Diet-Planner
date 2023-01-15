using Microsoft.EntityFrameworkCore.Migrations;

namespace DietPlanner.Api.Migrations
{
    public partial class add_product_ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMeals_Products_ProductId",
                table: "DailyMeals");

            migrationBuilder.DropIndex(
                name: "IX_DailyMeals_ProductId",
                table: "DailyMeals");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DailyMeals");

            migrationBuilder.AddColumn<string>(
                name: "ProductsId",
                table: "DailyMeals",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "DailyMeals");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DailyMeals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_ProductId",
                table: "DailyMeals",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMeals_Products_ProductId",
                table: "DailyMeals",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
