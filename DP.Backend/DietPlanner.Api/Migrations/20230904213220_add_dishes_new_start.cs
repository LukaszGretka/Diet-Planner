using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class add_dishes_new_start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealProducts");

            migrationBuilder.DropTable(
                name: "MealTypes");

            migrationBuilder.DropTable(
                name: "UserMeasurements");

            migrationBuilder.DropTable(
                name: "UserMeals");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExposedForOtherUsers",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    MealType = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false),
                    Chest = table.Column<decimal>(type: "TEXT", nullable: false),
                    Belly = table.Column<decimal>(type: "TEXT", nullable: false),
                    Waist = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ForearmRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    ForearmLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    ExposedForOtherUsers = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    PortionMultiplier = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 1m),
                    DishId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishProducts_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealDishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DishesId = table.Column<int>(type: "INTEGER", nullable: true),
                    MealsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealDishes_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MealDishes_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_Id",
                table: "Dishes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MealId",
                table: "Dishes",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_DishProducts_DishId",
                table: "DishProducts",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishProducts_ProductId",
                table: "DishProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_DishesId",
                table: "MealDishes",
                column: "DishesId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_Id",
                table: "MealDishes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealDishes_MealsId",
                table: "MealDishes",
                column: "MealsId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_Id",
                table: "Measurements",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishProducts");

            migrationBuilder.DropTable(
                name: "MealDishes");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ExposedForOtherUsers",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    MealTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Belly = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    BicepsRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalfRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    Chest = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    ForearmLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ForearmRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighLeft = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThighRight = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Waist = table.Column<decimal>(type: "TEXT", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    PortionMultiplier = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 1m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MealProducts_UserMeals_MealId",
                        column: x => x.MealId,
                        principalTable: "UserMeals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealTypes_UserMeals_Id",
                        column: x => x.Id,
                        principalTable: "UserMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealProducts_Id",
                table: "MealProducts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealProducts_MealId",
                table: "MealProducts",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_MealProducts_ProductId",
                table: "MealProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_Id",
                table: "MealTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasurements_Id",
                table: "UserMeasurements",
                column: "Id",
                unique: true);
        }
    }
}
