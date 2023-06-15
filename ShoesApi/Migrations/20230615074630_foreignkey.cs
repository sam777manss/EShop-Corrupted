using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesApi.Migrations
{
    public partial class foreignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgId",
                table: "ProductImageTable");

            migrationBuilder.DropIndex(
                name: "IX_ProductImageTable_ProductImgId",
                table: "ProductImageTable");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgGroupId",
                table: "ProductImageTable",
                column: "ProductImgGroupId",
                principalTable: "AddProductTable",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgGroupId",
                table: "ProductImageTable");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageTable_ProductImgId",
                table: "ProductImageTable",
                column: "ProductImgId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgId",
                table: "ProductImageTable",
                column: "ProductImgId",
                principalTable: "AddProductTable",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
