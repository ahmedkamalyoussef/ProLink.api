using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae2f8b53-2260-42d2-8c7d-40cb0df6a47c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3bdf5e6-d9e1-4648-bff8-c00bc5609dd8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e5bce0da-1bc5-4ae3-90f3-3b83d4593ec9", null, "Admin", "Admin" },
                    { "e7f94ded-1cff-44f5-b65b-64858facc2e1", null, "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5bce0da-1bc5-4ae3-90f3-3b83d4593ec9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f94ded-1cff-44f5-b65b-64858facc2e1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ae2f8b53-2260-42d2-8c7d-40cb0df6a47c", null, "Admin", "Admin" },
                    { "c3bdf5e6-d9e1-4648-bff8-c00bc5609dd8", null, "User", "User" }
                });
        }
    }
}
