using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations.FoodsDB
{
    public partial class Food2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems");

            migrationBuilder.RenameTable(
                name: "TodoItems",
                newName: "Foods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Foods",
                table: "Foods",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Foods",
                table: "Foods");

            migrationBuilder.RenameTable(
                name: "Foods",
                newName: "TodoItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems",
                column: "FoodId");
        }
    }
}
