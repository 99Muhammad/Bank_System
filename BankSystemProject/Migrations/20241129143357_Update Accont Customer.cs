using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccontCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "CustomersAccounts");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "CustomersAccounts");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "CustomersAccounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "CreditCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "CreditCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "CreditCards");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "CustomersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "CustomersAccounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "CustomersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
