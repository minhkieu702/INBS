using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminLogs_Users_AdminId",
                table: "AdminLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistAvailabilities_Users_ArtistId",
                table: "ArtistAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Users_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "UserBookings");

            migrationBuilder.DropTable(
                name: "UserWaitLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "WaitLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "WaitLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists",
                columns: new[] { "ArtistId", "CustomerId" });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Artists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkinTones",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RGPColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinTones", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkinToneID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_SkinTones_SkinToneID",
                        column: x => x.SkinToneID,
                        principalTable: "SkinTones",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NailDesignSkinTones",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkinToneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesignSkinTones", x => new { x.SkinToneId, x.DesignId });
                    table.ForeignKey(
                        name: "FK_NailDesignSkinTones_NailDesigns_DesignId",
                        column: x => x.DesignId,
                        principalTable: "NailDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NailDesignSkinTones_SkinTones_SkinToneId",
                        column: x => x.SkinToneId,
                        principalTable: "SkinTones",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteDesigns",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteDesigns", x => new { x.DesignId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_FavoriteDesigns_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteDesigns_NailDesigns_DesignId",
                        column: x => x.DesignId,
                        principalTable: "NailDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OccasionPreferences",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccasionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccasionPreferences", x => new { x.OccasionId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_OccasionPreferences_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OccasionPreferences_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_CustomerId",
                table: "WaitLists",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_UserId",
                table: "Artists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SkinToneID",
                table: "Customers",
                column: "SkinToneID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDesigns_CustomerId",
                table: "FavoriteDesigns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignSkinTones_DesignId",
                table: "NailDesignSkinTones",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_OccasionPreferences_CustomerId",
                table: "OccasionPreferences",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminLogs_Admins_AdminId",
                table: "AdminLogs",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistAvailabilities_Artists_ArtistId",
                table: "ArtistAvailabilities",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitLists_Artists_ArtistId",
                table: "WaitLists",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitLists_Customers_CustomerId",
                table: "WaitLists",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminLogs_Admins_AdminId",
                table: "AdminLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistAvailabilities_Artists_ArtistId",
                table: "ArtistAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_Artists_ArtistId",
                table: "WaitLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_Customers_CustomerId",
                table: "WaitLists");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "FavoriteDesigns");

            migrationBuilder.DropTable(
                name: "NailDesignSkinTones");

            migrationBuilder.DropTable(
                name: "OccasionPreferences");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "SkinTones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists");

            migrationBuilder.DropIndex(
                name: "IX_WaitLists_CustomerId",
                table: "WaitLists");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Bookings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "UserBookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsArtist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookings", x => new { x.BookingId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBookings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWaitLists",
                columns: table => new
                {
                    WaitListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsArtist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWaitLists", x => new { x.WaitListId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserWaitLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWaitLists_WaitLists_WaitListId",
                        column: x => x.WaitListId,
                        principalTable: "WaitLists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookings_UserId",
                table: "UserBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWaitLists_UserId",
                table: "UserWaitLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminLogs_Users_AdminId",
                table: "AdminLogs",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistAvailabilities_Users_ArtistId",
                table: "ArtistAvailabilities",
                column: "ArtistId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Users_CustomerId",
                table: "Recommendations",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
