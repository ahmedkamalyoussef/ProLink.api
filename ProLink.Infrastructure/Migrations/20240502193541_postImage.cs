using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class postImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3519df6f-044b-46a3-aa5f-1d2ef1f56a71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e2aaab1-2667-4a15-bf7f-b0d7edc71293");

            migrationBuilder.AddColumn<string>(
                name: "PostImage",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "947de63f-2695-4c99-9069-c29fb6ee4f6f", null, "Admin", "Admin" },
                    { "ccd7957e-d29d-4fdf-a6f1-af3a29acbd2a", null, "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "947de63f-2695-4c99-9069-c29fb6ee4f6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ccd7957e-d29d-4fdf-a6f1-af3a29acbd2a");

            migrationBuilder.DropColumn(
                name: "PostImage",
                table: "Posts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3519df6f-044b-46a3-aa5f-1d2ef1f56a71", null, "Admin", "Admin" },
                    { "5e2aaab1-2667-4a15-bf7f-b0d7edc71293", null, "User", "User" }
                });
        }
    }
}
