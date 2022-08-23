using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knjizara.Migrations
{
    public partial class UserFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysLate",
                table: "UserReturnBorrowedBookTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DaysLate",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LateFee",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysLate",
                table: "UserReturnBorrowedBookTransaction");

            migrationBuilder.DropColumn(
                name: "DaysLate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LateFee",
                table: "AspNetUsers");
        }
    }
}
