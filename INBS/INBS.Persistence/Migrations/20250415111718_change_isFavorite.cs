using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_isFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NailDesignServiceGuide");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "CustomerSelected");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Bookings");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "CustomerSelected",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "NailDesignServiceGuide",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NailDesignServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NailDesignServiceGuide", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NailDesignServiceGuide_NailDesignService_NailDesignServiceId",
                        column: x => x.NailDesignServiceId,
                        principalTable: "NailDesignService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NailDesignServiceGuide_NailDesignServiceId",
                table: "NailDesignServiceGuide",
                column: "NailDesignServiceId");
        }
    }
}
