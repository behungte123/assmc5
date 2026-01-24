using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab4.Migrations
{
    /// <inheritdoc />
    public partial class AddVnPayFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VnpPayDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VnpResponseCode",
                table: "Orders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VnpTransactionNo",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VnpTxnRef",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VnpPayDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VnpResponseCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VnpTransactionNo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VnpTxnRef",
                table: "Orders");
        }
    }
}
