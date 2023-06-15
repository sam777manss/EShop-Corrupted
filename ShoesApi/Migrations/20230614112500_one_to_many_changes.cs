using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesApi.Migrations
{
    public partial class one_to_many_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductId",
                table: "ProductImageTable");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AddProductTable");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductImageTable",
                newName: "ProductImgGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "ProductImageTable",
                newName: "ProductImgId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable",
                newName: "IX_ProductImageTable_ProductImgGroupId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AddProductTable",
                newName: "ProductImgGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgGroupId",
                table: "ProductImageTable",
                column: "ProductImgGroupId",
                principalTable: "AddProductTable",
                principalColumn: "ProductImgGroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgGroupId",
                table: "ProductImageTable");

            migrationBuilder.RenameColumn(
                name: "ProductImgGroupId",
                table: "ProductImageTable",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductImgId",
                table: "ProductImageTable",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImageTable_ProductImgGroupId",
                table: "ProductImageTable",
                newName: "IX_ProductImageTable_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductImgGroupId",
                table: "AddProductTable",
                newName: "ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "AddProductTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductId",
                table: "ProductImageTable",
                column: "ProductId",
                principalTable: "AddProductTable",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
