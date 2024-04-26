using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobRequests_JobRequestId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobRequestId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "caf15d26-071b-45aa-b874-724428f7403f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc287709-054d-42cb-91d2-80215f8aac50");

            migrationBuilder.DropColumn(
                name: "JobRequestId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobRequestUser",
                columns: table => new
                {
                    ReceivedJobRequestsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipientsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequestUser", x => new { x.ReceivedJobRequestsId, x.RecipientsId });
                    table.ForeignKey(
                        name: "FK_JobRequestUser_AspNetUsers_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRequestUser_JobRequests_ReceivedJobRequestsId",
                        column: x => x.ReceivedJobRequestsId,
                        principalTable: "JobRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e52ef0e-40e8-4802-8851-fcc4cbee419a", null, "User", "User" },
                    { "d55d4b00-1738-4e0c-8ae2-a7e22b6ec550", null, "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequestUser_RecipientsId",
                table: "JobRequestUser",
                column: "RecipientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "JobRequestUser");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e52ef0e-40e8-4802-8851-fcc4cbee419a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d55d4b00-1738-4e0c-8ae2-a7e22b6ec550");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "JobRequestId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "caf15d26-071b-45aa-b874-724428f7403f", "0", "User", "User" },
                    { "dc287709-054d-42cb-91d2-80215f8aac50", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobRequestId",
                table: "AspNetUsers",
                column: "JobRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobRequests_JobRequestId",
                table: "AspNetUsers",
                column: "JobRequestId",
                principalTable: "JobRequests",
                principalColumn: "Id");
        }
    }
}
