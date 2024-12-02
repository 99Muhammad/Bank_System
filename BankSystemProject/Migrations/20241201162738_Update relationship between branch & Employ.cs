using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdaterelationshipbetweenbranchEmploy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_UsersId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_UsersId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Branches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UsersId",
                table: "Branches",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_UsersId",
                table: "Branches",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
