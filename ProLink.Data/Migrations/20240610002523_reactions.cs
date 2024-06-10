using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Data.Migrations
{
    /// <inheritdoc />
    public partial class reactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId2",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId2",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId2",
                table: "Jobs",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId2",
                table: "Jobs",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
