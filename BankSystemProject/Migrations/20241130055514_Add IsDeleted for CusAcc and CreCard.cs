using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedforCusAccandCreCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "CreditCards",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "CustomersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomersAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "CustomersAccounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomersAccounts");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "CreditCards",
                newName: "AccountNumber");
        }
    }
}
