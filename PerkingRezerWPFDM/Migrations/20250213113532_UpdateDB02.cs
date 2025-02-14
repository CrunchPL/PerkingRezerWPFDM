using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerkingRezerWPFDM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Reservations",
                newName: "ReservationTime");

            migrationBuilder.RenameColumn(
                name: "ParkingSpot",
                table: "Reservations",
                newName: "ReservationDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Reservations",
                newName: "ParkingSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "ReservationTime",
                table: "Reservations",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "Reservations",
                newName: "ParkingSpot");

            migrationBuilder.RenameColumn(
                name: "ParkingSpotId",
                table: "Reservations",
                newName: "EndTime");
        }
    }
}
