using Microsoft.EntityFrameworkCore.Migrations;

namespace SuperMart.Dal.Migrations
{
    public partial class Migration_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Stores",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreName",
                table: "Stores",
                column: "StoreName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId_Name",
                table: "Products",
                columns: new[] { "StoreId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stores_StoreName",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Products_StoreId_Name",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Stores",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
