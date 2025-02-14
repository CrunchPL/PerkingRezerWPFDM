using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerkingRezerWPFDM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndMinute",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartMinute",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndMinute",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "StartMinute",
                table: "Reservations");
        }
    }
}
