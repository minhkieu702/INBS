using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class modifyDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "CustomDesignId",
                table: "AccessoryCustomDesigns",
                newName: "CustomNailDesignId");

            migrationBuilder.RenameIndex(
                name: "IX_AccessoryCustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns",
                newName: "IX_AccessoryCustomDesigns_CustomNailDesignId");

            migrationBuilder.CreateTable(
                name: "CustomNailDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "NailDesigns",
                columns: table => new
                {
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NailPosition = table.Column<int>(type: "int", nullable: false),
                    IsLeft = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesigns", x => new { x.DesignId, x.NailPosition, x.IsLeft });
                    table.ForeignKey(
                        name: "FK_NailDesigns_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomNailDesigns_CustomDesignId",
                table: "CustomNailDesigns",
                column: "CustomDesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomNailDesigns_CustomNailDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomNailDesignId",
                principalTable: "CustomNailDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomNailDesigns_CustomNailDesignId",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropTable(
                name: "CustomNailDesigns");

            migrationBuilder.DropTable(
                name: "NailDesigns");

            migrationBuilder.RenameColumn(
                name: "CustomNailDesignId",
                table: "AccessoryCustomDesigns",
                newName: "CustomDesignId");

            migrationBuilder.RenameIndex(
                name: "IX_AccessoryCustomDesigns_CustomNailDesignId",
                table: "AccessoryCustomDesigns",
                newName: "IX_AccessoryCustomDesigns_CustomDesignId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryCustomDesigns_CustomDesigns_CustomDesignId",
                table: "AccessoryCustomDesigns",
                column: "CustomDesignId",
                principalTable: "CustomDesigns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
