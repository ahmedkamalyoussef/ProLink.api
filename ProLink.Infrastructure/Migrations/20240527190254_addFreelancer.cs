using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFreelancer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId2",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "Posts",
                newName: "FreelancerId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId2",
                table: "Posts",
                newName: "IX_Posts_FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_FreelancerId",
                table: "Posts",
                column: "FreelancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_FreelancerId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "FreelancerId",
                table: "Posts",
                newName: "UserId2");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_FreelancerId",
                table: "Posts",
                newName: "IX_Posts_UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId2",
                table: "Posts",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
