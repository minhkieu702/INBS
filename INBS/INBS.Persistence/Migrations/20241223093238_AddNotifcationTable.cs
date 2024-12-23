using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class AddNotifcationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_Artists_ArtistId",
                table: "WaitLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_Customers_CustomerId",
                table: "WaitLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists");

            migrationBuilder.RenameTable(
                name: "WaitLists",
                newName: "WaitList");

            migrationBuilder.RenameIndex(
                name: "IX_WaitLists_CustomerId",
                table: "WaitList",
                newName: "IX_WaitList_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitList",
                table: "WaitList",
                columns: new[] { "ArtistId", "CustomerId" });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NotifyAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitList_Artists_ArtistId",
                table: "WaitList",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitList_Customers_CustomerId",
                table: "WaitList",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitList_Artists_ArtistId",
                table: "WaitList");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitList_Customers_CustomerId",
                table: "WaitList");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitList",
                table: "WaitList");

            migrationBuilder.RenameTable(
                name: "WaitList",
                newName: "WaitLists");

            migrationBuilder.RenameIndex(
                name: "IX_WaitList_CustomerId",
                table: "WaitLists",
                newName: "IX_WaitLists_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitLists",
                table: "WaitLists",
                columns: new[] { "ArtistId", "CustomerId" });

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
    }
}
