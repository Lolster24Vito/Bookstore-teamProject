using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knjizara.Migrations
{
    public partial class UserReturnBorrowedBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "BookUserBorrowTransaction");

            migrationBuilder.CreateTable(
                name: "UserReturnBorrowedBookTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowId = table.Column<int>(type: "int", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReturnBorrowedBookTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReturnBorrowedBookTransaction_BookUserBorrowTransaction_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "BookUserBorrowTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReturnBorrowedBookTransaction_BorrowId",
                table: "UserReturnBorrowedBookTransaction",
                column: "BorrowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReturnBorrowedBookTransaction");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "BookUserBorrowTransaction",
                type: "datetime2",
                nullable: true);
        }
    }
}
