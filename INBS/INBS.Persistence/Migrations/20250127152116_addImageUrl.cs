using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomNailDesigns");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CustomNailDesigns");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "CustomNailDesigns",
                newName: "IsLeft");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "NailDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CustomNailDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NailPosition",
                table: "CustomNailDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "NailDesigns");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CustomNailDesigns");

            migrationBuilder.DropColumn(
                name: "NailPosition",
                table: "CustomNailDesigns");

            migrationBuilder.RenameColumn(
                name: "IsLeft",
                table: "CustomNailDesigns",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomNailDesigns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "CustomNailDesigns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
