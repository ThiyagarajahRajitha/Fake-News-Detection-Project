using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class FixedForeignKeyIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewedBy",
                table: "ReviewRequest",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest",
                column: "ReviewedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewedBy",
                table: "ReviewRequest",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewRequest_Users_ReviewedBy",
                table: "ReviewRequest",
                column: "ReviewedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
