using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class FixSomething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Occasions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SkinTones",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HexCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinTones", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPreferences");

            migrationBuilder.DropTable(
                name: "DesignPreferences");

            migrationBuilder.DropTable(
                name: "Occasions");

            migrationBuilder.DropTable(
                name: "SkinTones");
        }
    }
}
