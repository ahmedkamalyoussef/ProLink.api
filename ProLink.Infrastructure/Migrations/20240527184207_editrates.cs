using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editrates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_AspNetUsers_RatedId",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_AspNetUsers_RaterId",
                table: "Rate");

            migrationBuilder.RenameColumn(
                name: "RatedId",
                table: "Rate",
                newName: "RatedPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_RatedId",
                table: "Rate",
                newName: "IX_Rate_RatedPostId");

            migrationBuilder.AddColumn<string>(
                name: "RateId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RateCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RateId",
                table: "Posts",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Rate_RateId",
                table: "Posts",
                column: "RateId",
                principalTable: "Rate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_AspNetUsers_RaterId",
                table: "Rate",
                column: "RaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Posts_RatedPostId",
                table: "Rate",
                column: "RatedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Rate_RateId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_AspNetUsers_RaterId",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Posts_RatedPostId",
                table: "Rate");

            migrationBuilder.DropIndex(
                name: "IX_Posts_RateId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RateCount",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RatedPostId",
                table: "Rate",
                newName: "RatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_RatedPostId",
                table: "Rate",
                newName: "IX_Rate_RatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_AspNetUsers_RatedId",
                table: "Rate",
                column: "RatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_AspNetUsers_RaterId",
                table: "Rate",
                column: "RaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
