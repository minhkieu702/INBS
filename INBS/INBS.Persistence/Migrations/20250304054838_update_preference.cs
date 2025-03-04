using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_preference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPreferences");

            migrationBuilder.DropTable(
                name: "DesignPreferences");

            migrationBuilder.RenameColumn(
                name: "Preferences",
                table: "Customers",
                newName: "Description");

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

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ServiceDate",
                table: "Bookings",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ServiceTime",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "FeedbackService",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackService", x => new { x.FeedbackId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_FeedbackService_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackService_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false),
                    PreferenceType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Preferences_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Preferences_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_FeedbackService_ServiceId",
                table: "FeedbackService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_CustomerId",
                table: "Preferences",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_DesignId",
                table: "Preferences",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Artists_ArtistId",
                table: "Feedbacks",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Artists_ArtistId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Designs_DesignId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Stores_StoreId",
                table: "Feedbacks");

            migrationBuilder.DropTable(
                name: "FeedbackService");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ArtistId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DesignId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_StoreId",
                table: "Feedbacks");

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
                name: "Type",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ServiceTime",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Customers",
                newName: "Preferences");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServiceDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerPreferences",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false),
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
                name: "DesignPreferences",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false),
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
        }
    }
}
