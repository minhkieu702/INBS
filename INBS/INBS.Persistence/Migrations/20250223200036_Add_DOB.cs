using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_DOB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Username");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Email");
        }
    }
}
