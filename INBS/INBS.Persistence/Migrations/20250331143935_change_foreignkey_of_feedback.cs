using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_foreignkey_of_feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Feedbacks",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                table: "Feedbacks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Feedbacks",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_CustomerId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
