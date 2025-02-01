using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationForAccessory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CustomDesigns");

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "AccessoryCustomDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "AccessoryCustomDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "AccessoryCustomDesigns");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "AccessoryCustomDesigns");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CustomDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
