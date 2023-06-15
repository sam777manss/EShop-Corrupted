using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesApi.Migrations
{
    public partial class one_to_one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "ProductImageTable",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductId",
                table: "ProductImageTable",
                column: "ProductId",
                principalTable: "AddProductTable",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImageTable_AddProductTable_ProductId",
                table: "ProductImageTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable");

            migrationBuilder.DropIndex(
                name: "IX_ProductImageTable_ProductId",
                table: "ProductImageTable");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "ProductImageTable",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImageTable",
                table: "ProductImageTable",
                column: "ProductId");
        }
    }
}
