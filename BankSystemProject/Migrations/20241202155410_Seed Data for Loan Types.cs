using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataforLoanTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoanTypes",
                columns: new[] { "LoanTypeId", "Description", "InterestRate", "LoanTypeName" },
                values: new object[,]
                {
                    { 1, "A personal loan with a fixed interest rate.", 5.5, "Personal Loan" },
                    { 2, "A loan to purchase a home with lower interest rates.", 3.7999999999999998, "Home Loan" },
                    { 3, "A loan to purchase a vehicle with medium interest rates.", 4.2000000000000002, "Auto Loan" },
                    { 4, "A loan to support higher education costs with subsidized interest rates.", 3.0, "Student Loan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 4);
        }
    }
}
