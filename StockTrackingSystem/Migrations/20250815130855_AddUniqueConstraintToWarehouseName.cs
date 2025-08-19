using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToWarehouseName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses");
        }
    }
}
