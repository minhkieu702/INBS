using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class ChangeRelationshipOfDeviceToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTokens_Users_UserId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTokens_Users_UserId",
                table: "DeviceTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
