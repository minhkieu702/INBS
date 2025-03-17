using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_preference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Customers_CustomerId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Designs_DesignId",
                table: "Preferences");

            migrationBuilder.AlterColumn<Guid>(
                name: "DesignId",
                table: "Preferences",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Preferences",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Customers_CustomerId",
                table: "Preferences",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Designs_DesignId",
                table: "Preferences",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Customers_CustomerId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Designs_DesignId",
                table: "Preferences");

            migrationBuilder.AlterColumn<Guid>(
                name: "DesignId",
                table: "Preferences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Preferences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Customers_CustomerId",
                table: "Preferences",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Designs_DesignId",
                table: "Preferences",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
