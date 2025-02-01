using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addPriceorCustomDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CustomDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CustomDesigns");
        }
    }
}
