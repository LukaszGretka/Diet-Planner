using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietPlanner.Api.Migrations
{
    public partial class adjust_database_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMeals");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealType",
                table: "MealType");

            migrationBuilder.RenameTable(
                name: "MealType",
                newName: "MealTypes");

            migrationBuilder.RenameColumn(
                name: "MealName",
                table: "MealTypes",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_MealType_Id",
                table: "MealTypes",
                newName: "IX_MealTypes_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MealTypes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealTypes",
                table: "MealTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    MealTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_MealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMeasurements",
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
                    CalfLeft = table.Column<decimal>(type: "TEXT", nullable: false)
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
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealProducts_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                name: "IX_Meals_Id",
                table: "Meals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MealTypeId",
                table: "Meals",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasurements_Id",
                table: "UserMeasurements",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealProducts");

            migrationBuilder.DropTable(
                name: "UserMeasurements");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealTypes",
                table: "MealTypes");

            migrationBuilder.RenameTable(
                name: "MealTypes",
                newName: "MealType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MealType",
                newName: "MealName");

            migrationBuilder.RenameIndex(
                name: "IX_MealTypes_Id",
                table: "MealType",
                newName: "IX_MealType_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MealType",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealType",
                table: "MealType",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DailyMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    ProductsId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyMeals_MealType_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
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
                    Waist = table.Column<decimal>(type: "TEXT", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_Id",
                table: "DailyMeals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_MealTypeId",
                table: "DailyMeals",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_Id",
                table: "Measurements",
                column: "Id",
                unique: true);
        }
    }
}
