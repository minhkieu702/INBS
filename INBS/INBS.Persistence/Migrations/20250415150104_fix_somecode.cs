using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INBS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_somecode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "NailDesignServiceSelecteds");

            migrationBuilder.AddColumn<long>(
                name: "AverageDuration",
                table: "NailDesignService",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDuration",
                table: "NailDesignService");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Feedbacks");

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "NailDesignServiceSelecteds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
