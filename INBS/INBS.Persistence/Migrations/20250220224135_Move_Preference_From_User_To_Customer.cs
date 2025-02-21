using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Move_Preference_From_User_To_Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
