using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaloonReservation.Migrations
{
    /// <inheritdoc />
    public partial class customerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "496b3d0a-9be4-47db-a361-42c8f85f0be7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee3fca84-e253-43de-80ec-e9d5f0e49560");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dc4fd558-5164-49f9-9131-05c27e28e325", null, "Barber", "BARBER" },
                    { "ed904523-361a-4d8d-aa34-d3f378ec817c", null, "Admin", "ADMIN" },
                    { "f5ebda2b-9ae0-4ad2-a7d4-70627c314785", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc4fd558-5164-49f9-9131-05c27e28e325");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed904523-361a-4d8d-aa34-d3f378ec817c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5ebda2b-9ae0-4ad2-a7d4-70627c314785");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "496b3d0a-9be4-47db-a361-42c8f85f0be7", null, "Admin", "ADMIN" },
                    { "ee3fca84-e253-43de-80ec-e9d5f0e49560", null, "Barber", "BARBER" }
                });
        }
    }
}
