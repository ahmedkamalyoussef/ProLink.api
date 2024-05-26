using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Skill",
                table: "AspNetUsers",
                newName: "Skills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "AspNetUsers",
                newName: "Skill");
        }
    }
}
