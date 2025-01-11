using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Domain
{
    /// <inheritdoc />
    public partial class EditNailDesignTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "NailDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "NailDesigns");
        }
    }
}
