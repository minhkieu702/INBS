using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_all_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_ID",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Stores_StoreId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_ID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomCombos_CustomComboId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomDesigns_CustomDesignId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Artists_ArtistId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Designs_DesignId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Stores_StoreId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Admins_AdminId",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "AccessoryCustomDesigns");

            migrationBuilder.DropTable(
                name: "ArtistAvailabilities");

            migrationBuilder.DropTable(
                name: "DesignServices");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ServiceCustomCombos");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "CustomNailDesigns");

            migrationBuilder.DropTable(
                name: "CustomCombos");

            migrationBuilder.DropTable(
                name: "CustomDesigns");

            migrationBuilder.DropIndex(
                name: "IX_Stores_AdminId",
                table: "Stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NailDesigns",
                table: "NailDesigns");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ArtistId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DesignId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_StoreId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Cancellations_BookingId",
                table: "Cancellations");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Artists_StoreId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Artists",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "RecommendedDesigns",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "RecommendedTimeSlots",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "NotifyAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Artists");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Recommendations",
                newName: "DesignId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Notifications",
                newName: "NotificationType");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Feedbacks",
                newName: "FeedbackType");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Feedbacks",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_CustomerId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_BookingId");

            migrationBuilder.RenameColumn(
                name: "ServiceTime",
                table: "Bookings",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "CustomDesignId",
                table: "Bookings",
                newName: "CustomerSelectedId");

            migrationBuilder.RenameColumn(
                name: "CustomComboId",
                table: "Bookings",
                newName: "ArtistStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomDesignId",
                table: "Bookings",
                newName: "IX_Bookings_CustomerSelectedId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomComboId",
                table: "Bookings",
                newName: "IX_Bookings_ArtistStoreId");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "NailDesigns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiry",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PredictEndTime",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                columns: new[] { "CustomerId", "DesignId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NailDesigns",
                table: "NailDesigns",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ArtistStores",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistStores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArtistStores_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSelected",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSelected", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerSelected_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    NumerialOrder = table.Column<int>(type: "int", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => new { x.DesignId, x.NumerialOrder });
                    table.ForeignKey(
                        name: "FK_Medias_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NailDesignService",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NailDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraPrice = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesignService", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NailDesignService_NailDesigns_NailDesignId",
                        column: x => x.NailDesignId,
                        principalTable: "NailDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NailDesignService_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NailDesignServiceSelecteds",
                columns: table => new
                {
                    CustomerSelectedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NailDesignServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesignServiceSelecteds", x => new { x.NailDesignServiceId, x.CustomerSelectedId });
                    table.ForeignKey(
                        name: "FK_NailDesignServiceSelecteds_CustomerSelected_CustomerSelectedId",
                        column: x => x.CustomerSelectedId,
                        principalTable: "CustomerSelected",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NailDesignServiceSelecteds_NailDesignService_NailDesignServiceId",
                        column: x => x.NailDesignServiceId,
                        principalTable: "NailDesignService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_DesignId",
                table: "Recommendations",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesigns_DesignId",
                table: "NailDesigns",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_BookingId",
                table: "Cancellations",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistStores_ArtistId",
                table: "ArtistStores",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistStores_StoreId",
                table: "ArtistStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSelected_CustomerID",
                table: "CustomerSelected",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignService_NailDesignId",
                table: "NailDesignService",
                column: "NailDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignService_ServiceId",
                table: "NailDesignService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignServiceSelecteds_CustomerSelectedId",
                table: "NailDesignServiceSelecteds",
                column: "CustomerSelectedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_ID",
                table: "Admins",
                column: "ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_ID",
                table: "Artists",
                column: "ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ArtistStores_ArtistStoreId",
                table: "Bookings",
                column: "ArtistStoreId",
                principalTable: "ArtistStores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CustomerSelected_CustomerSelectedId",
                table: "Bookings",
                column: "CustomerSelectedId",
                principalTable: "CustomerSelected",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Designs_DesignId",
                table: "Recommendations",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_ID",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_ID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ArtistStores_ArtistStoreId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomerSelected_CustomerSelectedId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Designs_DesignId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "ArtistStores");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "NailDesignServiceSelecteds");

            migrationBuilder.DropTable(
                name: "CustomerSelected");

            migrationBuilder.DropTable(
                name: "NailDesignService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_DesignId",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NailDesigns",
                table: "NailDesigns");

            migrationBuilder.DropIndex(
                name: "IX_NailDesigns_DesignId",
                table: "NailDesigns");

            migrationBuilder.DropIndex(
                name: "IX_Cancellations_BookingId",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "NailDesigns");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OtpExpiry",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PredictEndTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "DesignId",
                table: "Recommendations",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "Notifications",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "FeedbackType",
                table: "Feedbacks",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Feedbacks",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_CustomerId");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Bookings",
                newName: "ServiceTime");

            migrationBuilder.RenameColumn(
                name: "CustomerSelectedId",
                table: "Bookings",
                newName: "CustomDesignId");

            migrationBuilder.RenameColumn(
                name: "ArtistStoreId",
                table: "Bookings",
                newName: "CustomComboId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomerSelectedId",
                table: "Bookings",
                newName: "IX_Bookings_CustomDesignId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ArtistStoreId",
                table: "Bookings",
                newName: "IX_Bookings_CustomComboId");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "Stores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Artists",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Recommendations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Recommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Recommendations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RecommendedDesigns",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedTimeSlots",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NotifyAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DesignId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Bookings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageRating",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Artists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NailDesigns",
                table: "NailDesigns",
                columns: new[] { "DesignId", "NailPosition", "IsLeft" });

            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArtistAvailabilities",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BreakTime = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaximumBreakTime = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistAvailabilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArtistAvailabilities_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomCombos",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCombos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomCombos_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSave = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomDesigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomDesigns_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomDesigns_Designs_DesignID",
                        column: x => x.DesignID,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignServices",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraPrice = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignServices", x => new { x.ServiceId, x.DesignId });
                    table.ForeignKey(
                        name: "FK_DesignServices_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumerialOrder = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => new { x.DesignId, x.NumerialOrder });
                    table.ForeignKey(
                        name: "FK_Images_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCustomCombos",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCustomCombos", x => new { x.ServiceId, x.CustomComboId });
                    table.ForeignKey(
                        name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                        column: x => x.CustomComboId,
                        principalTable: "CustomCombos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceCustomCombos_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomNailDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLeft = table.Column<bool>(type: "bit", nullable: false),
                    NailPosition = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomNailDesigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomNailDesigns_CustomDesigns_CustomDesignId",
                        column: x => x.CustomDesignId,
                        principalTable: "CustomDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryCustomDesigns",
                columns: table => new
                {
                    AccessoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomNailDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryCustomDesigns", x => new { x.AccessoryId, x.CustomNailDesignId });
                    table.ForeignKey(
                        name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessoryCustomDesigns_CustomNailDesigns_CustomNailDesignId",
                        column: x => x.CustomNailDesignId,
                        principalTable: "CustomNailDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_AdminId",
                table: "Stores",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_CustomerId",
                table: "Recommendations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ArtistId",
                table: "Feedbacks",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DesignId",
                table: "Feedbacks",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_StoreId",
                table: "Feedbacks",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_BookingId",
                table: "Cancellations",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_StoreId",
                table: "Artists",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryCustomDesigns_CustomNailDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomNailDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAvailabilities_ArtistId",
                table: "ArtistAvailabilities",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomCombos_CustomerID",
                table: "CustomCombos",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesigns_CustomerID",
                table: "CustomDesigns",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesigns_DesignID",
                table: "CustomDesigns",
                column: "DesignID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomNailDesigns_CustomDesignId",
                table: "CustomNailDesigns",
                column: "CustomDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignServices_DesignId",
                table: "DesignServices",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCustomCombos_CustomComboId",
                table: "ServiceCustomCombos",
                column: "CustomComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_ID",
                table: "Admins",
                column: "ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Stores_StoreId",
                table: "Artists",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_ID",
                table: "Artists",
                column: "ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CustomCombos_CustomComboId",
                table: "Bookings",
                column: "CustomComboId",
                principalTable: "CustomCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CustomDesigns_CustomDesignId",
                table: "Bookings",
                column: "CustomDesignId",
                principalTable: "CustomDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Artists_ArtistId",
                table: "Feedbacks",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                table: "Feedbacks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Designs_DesignId",
                table: "Feedbacks",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Stores_StoreId",
                table: "Feedbacks",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Admins_AdminId",
                table: "Stores",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
