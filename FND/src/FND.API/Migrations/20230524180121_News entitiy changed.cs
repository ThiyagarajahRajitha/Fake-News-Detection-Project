using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class Newsentitiychanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "News",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "News",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "News",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "News",
                newName: "Description");
        }
    }
}
