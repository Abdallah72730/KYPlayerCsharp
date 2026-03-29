using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KYPlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_FanId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_FanId",
                table: "Ratings",
                column: "FanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_FanId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_FanId",
                table: "Ratings",
                column: "FanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
