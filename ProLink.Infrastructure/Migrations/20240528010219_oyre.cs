using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oyre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_Jobs_JobId",
                table: "JobRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_Jobs_JobId",
                table: "JobRequests",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_Jobs_JobId",
                table: "JobRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_Jobs_JobId",
                table: "JobRequests",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
