using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerkingRezerWPFDM.Migrations
{
    /// <inheritdoc />
    public partial class FixVehicleMakeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Make",
                table: "Vehicles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
