using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knjizara.Migrations
{
    public partial class UserBookLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserBook",
                columns: table => new
                {
                    BorrowedBooksId = table.Column<int>(type: "int", nullable: false),
                    UsersBorrowedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserBook", x => new { x.BorrowedBooksId, x.UsersBorrowedId });
                    table.ForeignKey(
                        name: "FK_AppUserBook_AspNetUsers_UsersBorrowedId",
                        column: x => x.UsersBorrowedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserBook_Books_BorrowedBooksId",
                        column: x => x.BorrowedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserBook1",
                columns: table => new
                {
                    PurchasedBooksId = table.Column<int>(type: "int", nullable: false),
                    UsersPurchasedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserBook1", x => new { x.PurchasedBooksId, x.UsersPurchasedId });
                    table.ForeignKey(
                        name: "FK_AppUserBook1_AspNetUsers_UsersPurchasedId",
                        column: x => x.UsersPurchasedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserBook1_Books_PurchasedBooksId",
                        column: x => x.PurchasedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserBook_UsersBorrowedId",
                table: "AppUserBook",
                column: "UsersBorrowedId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserBook1_UsersPurchasedId",
                table: "AppUserBook1",
                column: "UsersPurchasedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserBook");

            migrationBuilder.DropTable(
                name: "AppUserBook1");
        }
    }
}
