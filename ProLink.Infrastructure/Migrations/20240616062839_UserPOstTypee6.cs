using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPOstTypee6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserPostTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostTypes_PostId",
                table: "UserPostTypes",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserPostTypes_PostId",
                table: "UserPostTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPostTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPostTypes",
                table: "UserPostTypes",
                columns: new[] { "PostId", "UserId" });
        }
    }
}
