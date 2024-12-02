using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class Updateloantyptable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanTermMonths",
                table: "LoanTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EmploymentStatus",
                table: "LoanApplications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 1,
                column: "LoanTermMonths",
                value: 60);

            migrationBuilder.UpdateData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 2,
                column: "LoanTermMonths",
                value: 360);

            migrationBuilder.UpdateData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 3,
                column: "LoanTermMonths",
                value: 72);

            migrationBuilder.UpdateData(
                table: "LoanTypes",
                keyColumn: "LoanTypeId",
                keyValue: 4,
                column: "LoanTermMonths",
                value: 120);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanTermMonths",
                table: "LoanTypes");

            migrationBuilder.AlterColumn<string>(
                name: "EmploymentStatus",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
