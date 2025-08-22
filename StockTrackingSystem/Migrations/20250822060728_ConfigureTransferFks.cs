using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTransferFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_FromWarehouseId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_ToWarehouseId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_FromWarehouseId",
                table: "Transfers",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_ToWarehouseId",
                table: "Transfers",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_FromWarehouseId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_ToWarehouseId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_FromWarehouseId",
                table: "Transfers",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_ToWarehouseId",
                table: "Transfers",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
