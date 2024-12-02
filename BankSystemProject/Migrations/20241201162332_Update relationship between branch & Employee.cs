using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdaterelationshipbetweenbranchEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Employee_ManagerId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_ManagerId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Branches");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeSalary",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchID",
                table: "Employee",
                column: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branches_BranchID",
                table: "Employee",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branches_BranchID",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_BranchID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeSalary",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ManagerId",
                table: "Branches",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Employee_ManagerId",
                table: "Branches",
                column: "ManagerId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
