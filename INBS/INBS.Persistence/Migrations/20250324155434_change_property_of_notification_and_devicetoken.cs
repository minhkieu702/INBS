using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_property_of_notification_and_devicetoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTokens_Customers_CustomerId",
                table: "DeviceTokens");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "DeviceTokens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceTokens_CustomerId",
                table: "DeviceTokens",
                newName: "IX_DeviceTokens_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DeviceTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActiveAt",
                table: "DeviceTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "DeviceTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NailDesignServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.NailDesignServiceId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_NailDesignService_NailDesignServiceId",
                        column: x => x.NailDesignServiceId,
                        principalTable: "NailDesignService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTokens_Users_UserId",
                table: "DeviceTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTokens_Users_UserId",
                table: "DeviceTokens");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DeviceTokens");

            migrationBuilder.DropColumn(
                name: "LastActiveAt",
                table: "DeviceTokens");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "DeviceTokens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DeviceTokens",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceTokens_UserId",
                table: "DeviceTokens",
                newName: "IX_DeviceTokens_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTokens_Customers_CustomerId",
                table: "DeviceTokens",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
