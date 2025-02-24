using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSomeFIeldOfDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists");

            migrationBuilder.RenameColumn(
                name: "StoreID",
                table: "Artists",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Artists_StoreID",
                table: "Artists",
                newName: "IX_Artists_StoreId");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Stores_StoreId",
                table: "Artists",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Stores_StoreId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Artists");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Artists",
                newName: "StoreID");

            migrationBuilder.RenameIndex(
                name: "IX_Artists_StoreId",
                table: "Artists",
                newName: "IX_Artists_StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Stores_StoreID",
                table: "Artists",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
