using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class jop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24d8f2b3-816c-4c7f-8ede-620450ccb340");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ccbd6e8-8bbb-4a24-a027-de619a1a67f3");

            migrationBuilder.AddColumn<string>(
                name: "RecieverId",
                table: "JobRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "JobRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CV",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ae2f8b53-2260-42d2-8c7d-40cb0df6a47c", null, "Admin", "Admin" },
                    { "c3bdf5e6-d9e1-4648-bff8-c00bc5609dd8", null, "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_RecieverId",
                table: "JobRequests",
                column: "RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_AspNetUsers_RecieverId",
                table: "JobRequests",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_AspNetUsers_RecieverId",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_RecieverId",
                table: "JobRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae2f8b53-2260-42d2-8c7d-40cb0df6a47c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3bdf5e6-d9e1-4648-bff8-c00bc5609dd8");

            migrationBuilder.DropColumn(
                name: "RecieverId",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "CV",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24d8f2b3-816c-4c7f-8ede-620450ccb340", null, "Admin", "Admin" },
                    { "3ccbd6e8-8bbb-4a24-a027-de619a1a67f3", null, "User", "User" }
                });
        }
    }
}
