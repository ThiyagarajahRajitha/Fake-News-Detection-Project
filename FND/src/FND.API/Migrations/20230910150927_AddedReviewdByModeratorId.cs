using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedReviewdByModeratorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewedBy",
                table: "ReviewRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "ReviewRequest");
        }
    }
}
