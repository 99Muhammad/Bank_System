using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class deleteBalancfromCreditCaandaddittoCustomerAcco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "CreditCards");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "CustomersAccounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "CustomersAccounts");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "CreditCards",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
