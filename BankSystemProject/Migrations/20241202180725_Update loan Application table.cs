using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateloanApplicationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "LoanApplications",
                newName: "RepaymentSchedule");

            migrationBuilder.RenameColumn(
                name: "AmountRequested",
                table: "LoanApplications",
                newName: "LoanAmount");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantName",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationStatus",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CollateralDescription",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentStatus",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Income",
                table: "LoanApplications",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InterestRate",
                table: "LoanApplications",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecuredLoan",
                table: "LoanApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LoanTermMonths",
                table: "LoanApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ApplicantName",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "CollateralDescription",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Income",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "IsSecuredLoan",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "LoanTermMonths",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "RepaymentSchedule",
                table: "LoanApplications",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LoanAmount",
                table: "LoanApplications",
                newName: "AmountRequested");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "LoanApplications",
                type: "datetime2",
                nullable: true);
        }
    }
}
