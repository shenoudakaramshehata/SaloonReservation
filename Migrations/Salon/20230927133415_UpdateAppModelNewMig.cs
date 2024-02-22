using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class UpdateAppModelNewMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FattorahPaymentId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FattorahPaymentId",
                table: "Appointments");
        }
    }
}
