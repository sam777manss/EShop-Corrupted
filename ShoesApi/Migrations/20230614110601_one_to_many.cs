using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesApi.Migrations
{
    public partial class one_to_many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable",
                column: "ProductId",
                unique: true);
        }
    }
}
