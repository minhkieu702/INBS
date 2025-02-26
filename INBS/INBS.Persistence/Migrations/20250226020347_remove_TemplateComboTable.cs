using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class remove_TemplateComboTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTemplateCombos");

            migrationBuilder.DropTable(
                name: "TemplateCombos");

            migrationBuilder.DropColumn(
                name: "NumerialOrder",
                table: "ServiceCustomCombos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumerialOrder",
                table: "ServiceCustomCombos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TemplateCombos",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCombos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTemplateCombos",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumerialOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTemplateCombos", x => new { x.ServiceId, x.TemplateComboId });
                    table.ForeignKey(
                        name: "FK_ServiceTemplateCombos_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateCombos_TemplateCombos_TemplateComboId",
                        column: x => x.TemplateComboId,
                        principalTable: "TemplateCombos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateCombos_TemplateComboId",
                table: "ServiceTemplateCombos",
                column: "TemplateComboId");
        }
    }
}
