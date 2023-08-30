using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsDivAndLastFetchedNewsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastFetchedNewsUrl",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsDiv",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchedNewsUrl",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "NewsDiv",
                table: "Publications");
        }
    }
}
