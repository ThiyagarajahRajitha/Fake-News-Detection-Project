using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FND.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedPublisherRejectReasonANDModeratorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "Moderators",
                newName: "IsUpdated");

            migrationBuilder.AddColumn<string>(
                name: "PublisherRejectReason",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Moderators",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublisherRejectReason",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Moderators");

            migrationBuilder.RenameColumn(
                name: "IsUpdated",
                table: "Moderators",
                newName: "IsAccepted");
        }
    }
}
