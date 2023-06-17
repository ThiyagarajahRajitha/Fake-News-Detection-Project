using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "News",
                newName: "Publisher_id");

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "News",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "News",
                newName: "Publisher");

            migrationBuilder.RenameColumn(
                name: "Publisher_id",
                table: "News",
                newName: "Topic");
        }
    }
}
