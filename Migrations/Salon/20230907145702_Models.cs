using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentStatuses",
                columns: table => new
                {
                    AppointmentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentStatusTitleEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentStatusTitleAR = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatuses", x => x.AppointmentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityTLEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityTLAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderTLAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderTLEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "WeekDays",
                columns: table => new
                {
                    WeekDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDayIndex = table.Column<int>(type: "int", nullable: true),
                    WeekDayTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDays", x => x.WeekDayId);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaTLAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaTLEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                    table.ForeignKey(
                        name: "FK_Areas_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    serviceTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OneKidPrice = table.Column<double>(type: "float", nullable: false),
                    MoreKidsPrice = table.Column<double>(type: "float", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Services_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    BarberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OffWeekDayId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.BarberId);
                    table.ForeignKey(
                        name: "FK_Barbers_WeekDays_OffWeekDayId",
                        column: x => x.OffWeekDayId,
                        principalTable: "WeekDays",
                        principalColumn: "WeekDayId");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MapLocation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_Customers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId");
                });

            migrationBuilder.CreateTable(
                name: "BarberImages",
                columns: table => new
                {
                    BarberImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarberId = table.Column<int>(type: "int", nullable: false),
                    picDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarberImages", x => x.BarberImageId);
                    table.ForeignKey(
                        name: "FK_BarberImages_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentStatusId = table.Column<int>(type: "int", nullable: false),
                    AppointmentCreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<float>(type: "real", nullable: true),
                    BaberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                        column: x => x.AppointmentStatusId,
                        principalTable: "AppointmentStatuses",
                        principalColumn: "AppointmentStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Barbers_BaberId",
                        column: x => x.BaberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId");
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentServices",
                columns: table => new
                {
                    AppointmentServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentServices", x => x.AppointmentServiceId);
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId");
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppointmentStatuses",
                columns: new[] { "AppointmentStatusId", "AppointmentStatusTitleAR", "AppointmentStatusTitleEN" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "مغلق", "Closed" },
                    { 3, "ملغي", "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "BarberId", "Description", "FullName", "Image", "IsActive", "OffWeekDayId", "Phone" },
                values: new object[,]
                {
                    { 1, "good barber good barber good barber good barber good barber good barber", "Barber 1", "1.jpg", true, null, "258745874" },
                    { 2, "good barber good barber good barber good barber good barber good barber", "Barber 2", "2.jpg", true, null, "685784845" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "AreaId", "CityId", "CountryId", "Email", "FullAddress", "FullName", "Lat", "Lng", "MapLocation", "Phone" },
                values: new object[] { 1, null, null, 0, "mail@site.com", "Kuwait -sharq - block 2, st 133", "Customer 1", null, null, null, "5587485778" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderId", "GenderTLAr", "GenderTLEn" },
                values: new object[,]
                {
                    { 1, "ولد", "Boy" },
                    { 2, "بنت", "Girl" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "GenderId", "MoreKidsPrice", "OneKidPrice", "serviceTlAr", "serviceTlEn" },
                values: new object[,]
                {
                    { 1, 1, 6.0, 4.0, "قص الشعر", "Hair Cut" },
                    { 2, 1, 6.0, 4.0, "تقليم الشعر", "Hair Trim" },
                    { 3, 1, 4.0, 2.0, "تقليم الأمام", "Bang Trim" },
                    { 4, 1, 6.0, 4.0, "تصفيف الشعر - قصير", "Blowing-Short" },
                    { 5, 1, 8.0, 6.0, "تصفيف الشعر - متوسط", "Blowing-Medium" },
                    { 6, 1, 10.0, 8.0, "تصفيف الشعر - طويل", "Blowing-Long" },
                    { 7, 1, 6.0, 6.0, "تسريح الشعر بالمكواة - قصير", "Hair-Iron-Short" },
                    { 8, 1, 8.0, 8.0, "تسريح الشعر بالمكواة - متوسط", "Hair-Iron-Medium" },
                    { 9, 1, 10.0, 10.0, "تسريح الشعر بالمكواة - طويل", "Hair-Iron-Long" },
                    { 10, 1, 1.0, 1.0, "غسيل الشعر", "Hair Wash" },
                    { 11, 1, 5.0, 5.0, "علاج الشعر بالزيوت الطبيعية", "Hair Treatment With Natural Oils" },
                    { 12, 2, 6.0, 4.0, "قص الشعر", "Hair Cut" },
                    { 13, 2, 1.0, 1.0, "غسيل الشعر", "Hair Wash" },
                    { 14, 2, 4.0, 4.0, "قص الشعر للحالات الخاصة", "Special Needs Hair Cut" },
                    { 15, 2, 2.0, 2.0, "حلاقة الكبار", "Adult Shaving" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentStatusId",
                table: "Appointments",
                column: "AppointmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BaberId",
                table: "Appointments",
                column: "BaberId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_AppointmentId",
                table: "AppointmentServices",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_GenderId",
                table: "AppointmentServices",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_ServiceId",
                table: "AppointmentServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CityId",
                table: "Areas",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarberImages_BarberId",
                table: "BarberImages",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_OffWeekDayId",
                table: "Barbers",
                column: "OffWeekDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AreaId",
                table: "Customers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CityId",
                table: "Customers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_GenderId",
                table: "Services",
                column: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentServices");

            migrationBuilder.DropTable(
                name: "BarberImages");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AppointmentStatuses");

            migrationBuilder.DropTable(
                name: "Barbers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "WeekDays");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
