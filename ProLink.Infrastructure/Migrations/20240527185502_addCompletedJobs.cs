using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCompletedJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId2",
                table: "Posts",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId2",
                table: "Posts",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId2",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId2",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "Posts");
        }
    }
}
