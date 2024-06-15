using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Data.Migrations
{
    /// <inheritdoc />
    public partial class jobsupda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobTypes_AspNetUsers_JobId",
                table: "UserJobTypes");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypes_UserId",
                table: "UserJobTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobTypes_AspNetUsers_UserId",
                table: "UserJobTypes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobTypes_AspNetUsers_UserId",
                table: "UserJobTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserJobTypes_UserId",
                table: "UserJobTypes");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobTypes_AspNetUsers_JobId",
                table: "UserJobTypes",
                column: "JobId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
