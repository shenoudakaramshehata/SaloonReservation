using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class ModifyServiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 1,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 2,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 3,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 4,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 5,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 6,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 7,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 8,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 9,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 10,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 11,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 12,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 13,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 14,
                column: "Duration",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceId",
                keyValue: 15,
                column: "Duration",
                value: 0);

            migrationBuilder.InsertData(
                table: "WeekDays",
                columns: new[] { "WeekDayId", "WeekDayIndex", "WeekDayTitle" },
                values: new object[,]
                {
                    { 11, 1, "Monday" },
                    { 21, 2, "Tuesday" },
                    { 31, 3, "Wednesday" },
                    { 41, 4, "Thursday" },
                    { 51, 5, "Friday" },
                    { 61, 6, "Saturday" },
                    { 71, 7, "Sunday" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WeekDays",
                keyColumn: "WeekDayId",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Services");
        }
    }
}
