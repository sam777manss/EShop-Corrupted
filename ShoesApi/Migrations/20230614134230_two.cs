using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesApi.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ProductImageTable");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductImageTable",
                newName: "ProductImgId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "AddProductTable",
                newName: "ProductImgGroupId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductImgGroupId",
                table: "ProductImageTable",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable",
                column: "ProductImgGroupId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductImgId",
                table: "ProductImageTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable");

            migrationBuilder.DropIndex(
                name: "IX_ProductImageTable_ProductImgId",
                table: "ProductImageTable");

            migrationBuilder.DropColumn(
                name: "ProductImgGroupId",
                table: "ProductImageTable");

            migrationBuilder.RenameColumn(
                name: "ProductImgId",
                table: "ProductImageTable",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductImgGroupId",
                table: "AddProductTable",
                newName: "GroupId");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "ProductImageTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable",
                column: "ProductId");
        }
    }
}
