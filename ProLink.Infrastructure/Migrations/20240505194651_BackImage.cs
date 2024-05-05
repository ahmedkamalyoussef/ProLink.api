using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5bce0da-1bc5-4ae3-90f3-3b83d4593ec9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f94ded-1cff-44f5-b65b-64858facc2e1");

            migrationBuilder.AddColumn<string>(
                name: "BackImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "228dc1b0-3b77-437f-9fa7-337ae2799980", null, "User", "User" },
                    { "a1ffcc1d-cd2d-478c-b2bb-6f541bb95edb", null, "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "228dc1b0-3b77-437f-9fa7-337ae2799980");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1ffcc1d-cd2d-478c-b2bb-6f541bb95edb");

            migrationBuilder.DropColumn(
                name: "BackImage",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e5bce0da-1bc5-4ae3-90f3-3b83d4593ec9", null, "Admin", "Admin" },
                    { "e7f94ded-1cff-44f5-b65b-64858facc2e1", null, "User", "User" }
                });
        }
    }
}
