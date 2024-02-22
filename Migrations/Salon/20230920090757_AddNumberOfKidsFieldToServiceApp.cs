using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class AddNumberOfKidsFieldToServiceApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfKids",
                table: "TemporaryAppointmentServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfKids",
                table: "AppointmentServices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfKids",
                table: "TemporaryAppointmentServices");

            migrationBuilder.DropColumn(
                name: "NumberOfKids",
                table: "AppointmentServices");
        }
    }
}
