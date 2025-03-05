using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_duration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AverageDuration",
                table: "Services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "ServiceCustomCombos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AverageDuration",
                table: "Designs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "CustomNailDesigns",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDuration",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ServiceCustomCombos");

            migrationBuilder.DropColumn(
                name: "AverageDuration",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "CustomNailDesigns");
        }
    }
}
