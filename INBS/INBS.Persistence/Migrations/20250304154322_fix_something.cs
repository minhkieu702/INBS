using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_something : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings");

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

            migrationBuilder.RenameColumn(
                name: "ArtistAvailabilityId",
                table: "Bookings",
                newName: "ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ArtistAvailabilityId",
                table: "Bookings",
                newName: "IX_Bookings_ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings");

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

            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "Bookings",
                newName: "ArtistAvailabilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings",
                newName: "IX_Bookings_ArtistAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings",
                column: "ArtistAvailabilityId",
                principalTable: "ArtistAvailabilities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
