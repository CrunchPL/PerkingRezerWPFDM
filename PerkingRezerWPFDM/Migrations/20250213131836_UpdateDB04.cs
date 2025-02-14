using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerkingRezerWPFDM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationTime",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "EndHour",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartHour",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHour",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "Reservations");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ReservationTime",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
