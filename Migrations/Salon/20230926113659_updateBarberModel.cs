using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class updateBarberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CountryIsActive",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityOrderIndex",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "CityIsActive",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Barbers",
                keyColumn: "BarberId",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Barbers",
                keyColumn: "BarberId",
                keyValue: 2,
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Barbers");

            migrationBuilder.AlterColumn<bool>(
                name: "CountryIsActive",
                table: "Countries",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "CityOrderIndex",
                table: "Cities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "CityIsActive",
                table: "Cities",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
