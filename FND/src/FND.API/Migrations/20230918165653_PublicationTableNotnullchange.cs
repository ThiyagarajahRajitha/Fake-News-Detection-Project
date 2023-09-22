using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class PublicationTableNotnullchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PublisherRejectReason",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRequest_ReviewedBy",
                table: "ReviewRequest",
                column: "ReviewedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest",
                column: "ReviewedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest");

            migrationBuilder.DropIndex(
                name: "IX_ReviewRequest_ReviewedBy",
                table: "ReviewRequest");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherRejectReason",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
