using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class BigUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Artists_ArtistId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_NailDesigns_DesignId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SkinTones_SkinToneID",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_NailDesigns_DesignId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "FavoriteDesigns");

            migrationBuilder.DropTable(
                name: "NailDesignOccasions");

            migrationBuilder.DropTable(
                name: "NailDesignSkinTones");

            migrationBuilder.DropTable(
                name: "OccasionPreferences");

            migrationBuilder.DropTable(
                name: "WaitList");

            migrationBuilder.DropTable(
                name: "NailDesigns");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SkinToneID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SkinToneID",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "RGPColor",
                table: "SkinTones",
                newName: "HexCode");

            migrationBuilder.RenameColumn(
                name: "DesignId",
                table: "Bookings",
                newName: "CustomDesignId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Bookings",
                newName: "CustomComboId");

            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "Bookings",
                newName: "ArtistAvailabilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_DesignId",
                table: "Bookings",
                newName: "IX_Bookings_CustomDesignId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                newName: "IX_Bookings_CustomComboId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ArtistId",
                table: "Bookings",
                newName: "IX_Bookings_ArtistAvailabilityId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SkinTones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreID",
                table: "Artists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HexCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomCombos",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCombos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomCombos_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPreferences",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceType = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPreferences", x => new { x.CustomerId, x.PreferenceId, x.PreferenceType });
                    table.ForeignKey(
                        name: "FK_CustomerPreferences_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Designs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrendScore = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stores_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateCombos",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCombos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSave = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DesignPreferences",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceType = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignPreferences", x => new { x.DesignId, x.PreferenceId, x.PreferenceType });
                    table.ForeignKey(
                        name: "FK_DesignPreferences_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCustomCombos",
                columns: table => new
                {
                    CustomComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCustomCombos", x => new { x.ServiceId, x.CustomComboId });
                    table.ForeignKey(
                        name: "FK_ServiceCustomCombos_CustomCombos_CustomComboId",
                        column: x => x.CustomComboId,
                        principalTable: "CustomCombos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceCustomCombos_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreDesigns",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDesigns", x => new { x.StoreId, x.DesignId });
                    table.ForeignKey(
                        name: "FK_StoreDesigns_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreDesigns_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreServices",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreServices", x => new { x.StoreId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_StoreServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreServices_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTemplateCombos",
                columns: table => new
                {
                    TemplateComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTemplateCombos", x => new { x.ServiceId, x.TemplateComboId });
                    table.ForeignKey(
                        name: "FK_ServiceTemplateCombos_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                        column: x => x.TemplateComboId,
                        principalTable: "TemplateCombos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryCustomDesigns",
                columns: table => new
                {
                    AccessoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryCustomDesigns", x => new { x.AccessoryId, x.CustomDesignId });
                    table.ForeignKey(
                        name: "FK_AccessoryCustomDesigns_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                        column: x => x.CustomDesignId,
                        principalTable: "CustomDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_StoreID",
                table: "Artists",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryCustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomDesignId");

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
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCustomCombos_CustomComboId",
                table: "ServiceCustomCombos",
                column: "CustomComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos",
                column: "TemplateComboId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDesigns_DesignId",
                table: "StoreDesigns",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_AdminId",
                table: "Stores",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreServices_ServiceId",
                table: "StoreServices",
                column: "ServiceId");

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
                name: "FK_Customers_Users_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "Users",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Customers_Users_UserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Designs_DesignId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "AccessoryCustomDesigns");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "CustomerPreferences");

            migrationBuilder.DropTable(
                name: "DesignPreferences");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "PaintTypes");

            migrationBuilder.DropTable(
                name: "ServiceCustomCombos");

            migrationBuilder.DropTable(
                name: "ServiceTemplateCombos");

            migrationBuilder.DropTable(
                name: "StoreDesigns");

            migrationBuilder.DropTable(
                name: "StoreServices");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "CustomDesigns");

            migrationBuilder.DropTable(
                name: "CustomCombos");

            migrationBuilder.DropTable(
                name: "TemplateCombos");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Designs");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Artists_StoreID",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Artists");

            migrationBuilder.RenameColumn(
                name: "HexCode",
                table: "SkinTones",
                newName: "RGPColor");

            migrationBuilder.RenameColumn(
                name: "CustomDesignId",
                table: "Bookings",
                newName: "DesignId");

            migrationBuilder.RenameColumn(
                name: "CustomComboId",
                table: "Bookings",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ArtistAvailabilityId",
                table: "Bookings",
                newName: "ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomDesignId",
                table: "Bookings",
                newName: "IX_Bookings_DesignId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomComboId",
                table: "Bookings",
                newName: "IX_Bookings_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ArtistAvailabilityId",
                table: "Bookings",
                newName: "IX_Bookings_ArtistId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SkinTones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SkinToneID",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdminLogs_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NailDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    TrendScore = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesigns", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OccasionPreferences",
                columns: table => new
                {
                    OccasionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "WaitList",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitList", x => new { x.ArtistId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_WaitList_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaitList_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "NailDesignOccasions",
                columns: table => new
                {
                    OccasionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesignOccasions", x => new { x.OccasionId, x.DesignId });
                    table.ForeignKey(
                        name: "FK_NailDesignOccasions_NailDesigns_DesignId",
                        column: x => x.DesignId,
                        principalTable: "NailDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NailDesignOccasions_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NailDesignSkinTones",
                columns: table => new
                {
                    SkinToneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SkinToneID",
                table: "Customers",
                column: "SkinToneID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLogs_AdminId",
                table: "AdminLogs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDesigns_CustomerId",
                table: "FavoriteDesigns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignOccasions_DesignId",
                table: "NailDesignOccasions",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignSkinTones_DesignId",
                table: "NailDesignSkinTones",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_OccasionPreferences_CustomerId",
                table: "OccasionPreferences",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitList_CustomerId",
                table: "WaitList",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
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
                name: "FK_Bookings_NailDesigns_DesignId",
                table: "Bookings",
                column: "DesignId",
                principalTable: "NailDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SkinTones_SkinToneID",
                table: "Customers",
                column: "SkinToneID",
                principalTable: "SkinTones",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_NailDesigns_DesignId",
                table: "Images",
                column: "DesignId",
                principalTable: "NailDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Customers_CustomerId",
                table: "Recommendations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
