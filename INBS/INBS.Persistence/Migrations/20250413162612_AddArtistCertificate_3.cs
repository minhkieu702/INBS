using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistCertificate_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCertificate_Artists_ArtistId",
                table: "ArtistCertificate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistCertificate",
                table: "ArtistCertificate");

            migrationBuilder.RenameTable(
                name: "ArtistCertificate",
                newName: "ArtistCertificates");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistCertificate_ArtistId",
                table: "ArtistCertificates",
                newName: "IX_ArtistCertificates_ArtistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistCertificates",
                table: "ArtistCertificates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCertificates_Artists_ArtistId",
                table: "ArtistCertificates",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCertificates_Artists_ArtistId",
                table: "ArtistCertificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistCertificates",
                table: "ArtistCertificates");

            migrationBuilder.RenameTable(
                name: "ArtistCertificates",
                newName: "ArtistCertificate");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistCertificates_ArtistId",
                table: "ArtistCertificate",
                newName: "IX_ArtistCertificate_ArtistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistCertificate",
                table: "ArtistCertificate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCertificate_Artists_ArtistId",
                table: "ArtistCertificate",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
