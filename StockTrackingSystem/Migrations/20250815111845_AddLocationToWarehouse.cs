using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Warehouses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Warehouses");
        }
    }
}
