using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class UpdateSomething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomCombos_CustomComboId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomDesigns_CustomDesignId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomCombos_Customers_CustomerID",
                table: "CustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomDesigns_Designs_DesignID",
                table: "CustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Designs_DesignId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                table: "ServiceCustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCustomCombos_Services_ServiceId",
                table: "ServiceCustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateCombos_Services_ServiceId",
                table: "ServiceTemplateCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreDesigns_Designs_DesignId",
                table: "StoreDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreDesigns_Stores_StoreId",
                table: "StoreDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreServices_Services_ServiceId",
                table: "StoreServices");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreServices_Stores_StoreId",
                table: "StoreServices");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Users",
                newName: "LastModifiedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TemplateCombos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TemplateCombos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "TemplateCombos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Designs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Designs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomDesigns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomDesigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "CustomDesigns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomCombos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomCombos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "CustomCombos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Artists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Artists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ArtistAvailabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ArtistAvailabilities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "ArtistAvailabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Accessories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accessories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Accessories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CategoryServices",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryServices", x => new { x.CategoryId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_CategoryServices_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryServices_ServiceId",
                table: "CategoryServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                table: "AccessoryCustomDesigns",
                column: "AccessoryId",
                principalTable: "Accessories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomDesignId",
                principalTable: "CustomDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_UserId",
                table: "Artists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings",
                column: "ArtistAvailabilityId",
                principalTable: "ArtistAvailabilities",
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
                name: "FK_CustomCombos_Customers_CustomerID",
                table: "CustomCombos",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomDesigns_Designs_DesignID",
                table: "CustomDesigns",
                column: "DesignID",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Designs_DesignId",
                table: "Images",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                table: "ServiceCustomCombos",
                column: "CustomComboId",
                principalTable: "CustomCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCustomCombos_Services_ServiceId",
                table: "ServiceCustomCombos",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateCombos_Services_ServiceId",
                table: "ServiceTemplateCombos",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos",
                column: "TemplateComboId",
                principalTable: "TemplateCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreDesigns_Designs_DesignId",
                table: "StoreDesigns",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreDesigns_Stores_StoreId",
                table: "StoreDesigns",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreServices_Services_ServiceId",
                table: "StoreServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreServices_Stores_StoreId",
                table: "StoreServices",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomCombos_CustomComboId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CustomDesigns_CustomDesignId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomCombos_Customers_CustomerID",
                table: "CustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomDesigns_Designs_DesignID",
                table: "CustomDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Designs_DesignId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                table: "ServiceCustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCustomCombos_Services_ServiceId",
                table: "ServiceCustomCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateCombos_Services_ServiceId",
                table: "ServiceTemplateCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreDesigns_Designs_DesignId",
                table: "StoreDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreDesigns_Stores_StoreId",
                table: "StoreDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreServices_Services_ServiceId",
                table: "StoreServices");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreServices_Stores_StoreId",
                table: "StoreServices");

            migrationBuilder.DropTable(
                name: "CategoryServices");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TemplateCombos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TemplateCombos");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "TemplateCombos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Services");

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
                name: "CreatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomDesigns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomDesigns");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CustomDesigns");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomCombos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomCombos");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CustomCombos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ArtistAvailabilities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ArtistAvailabilities");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ArtistAvailabilities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Accessories");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Users",
                newName: "CreateAt");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                table: "AccessoryCustomDesigns",
                column: "AccessoryId",
                principalTable: "Accessories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomDesignId",
                principalTable: "CustomDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_UserId",
                table: "Artists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ArtistAvailabilities_ArtistAvailabilityId",
                table: "Bookings",
                column: "ArtistAvailabilityId",
                principalTable: "ArtistAvailabilities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CustomCombos_CustomComboId",
                table: "Bookings",
                column: "CustomComboId",
                principalTable: "CustomCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CustomDesigns_CustomDesignId",
                table: "Bookings",
                column: "CustomDesignId",
                principalTable: "CustomDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomCombos_Customers_CustomerID",
                table: "CustomCombos",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomDesigns_Designs_DesignID",
                table: "CustomDesigns",
                column: "DesignID",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Designs_DesignId",
                table: "Images",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                table: "ServiceCustomCombos",
                column: "CustomComboId",
                principalTable: "CustomCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCustomCombos_Services_ServiceId",
                table: "ServiceCustomCombos",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateCombos_Services_ServiceId",
                table: "ServiceTemplateCombos",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos",
                column: "TemplateComboId",
                principalTable: "TemplateCombos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreDesigns_Designs_DesignId",
                table: "StoreDesigns",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreDesigns_Stores_StoreId",
                table: "StoreDesigns",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreServices_Services_ServiceId",
                table: "StoreServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreServices_Stores_StoreId",
                table: "StoreServices",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
