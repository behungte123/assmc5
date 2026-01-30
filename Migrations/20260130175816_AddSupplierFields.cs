using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab4.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "PurchaseOrderItems",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PurchaseOrderItems",
                newName: "QuantityReceived");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultDiscount",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOrdered",
                table: "PurchaseOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PerformedByUserId",
                table: "InventoryLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDiscount",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "QuantityOrdered",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PerformedByUserId",
                table: "InventoryLogs");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "PurchaseOrderItems",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "QuantityReceived",
                table: "PurchaseOrderItems",
                newName: "Quantity");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
