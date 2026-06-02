using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSales.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Users_UserId",
                table: "Favourites");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Users_UserId",
                table: "Favourites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Users_UserId",
                table: "Favourites");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Users_UserId",
                table: "Favourites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
