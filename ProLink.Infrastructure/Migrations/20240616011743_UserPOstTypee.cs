using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPOstTypee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPostType_AspNetUsers_UserId",
                table: "UserPostType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostType_Post_PostId",
                table: "UserPostType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPostType",
                table: "UserPostType");

            migrationBuilder.RenameTable(
                name: "UserPostType",
                newName: "UserPostTypes");

            migrationBuilder.RenameIndex(
                name: "IX_UserPostType_UserId",
                table: "UserPostTypes",
                newName: "IX_UserPostTypes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostTypes_AspNetUsers_UserId",
                table: "UserPostTypes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostTypes_Post_PostId",
                table: "UserPostTypes",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPostTypes_AspNetUsers_UserId",
                table: "UserPostTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostTypes_Post_PostId",
                table: "UserPostTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes");

            migrationBuilder.RenameTable(
                name: "UserPostTypes",
                newName: "UserPostType");

            migrationBuilder.RenameIndex(
                name: "IX_UserPostTypes_UserId",
                table: "UserPostType",
                newName: "IX_UserPostType_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPostType",
                table: "UserPostType",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostType_AspNetUsers_UserId",
                table: "UserPostType",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostType_Post_PostId",
                table: "UserPostType",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
