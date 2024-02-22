using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class addTemporaryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "PublicSections",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DescritpionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DescritpionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PublicSections", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "TemporaryAppointments",
                columns: table => new
                {
                    TemporaryAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentStatusId = table.Column<int>(type: "int", nullable: false),
                    AppointmentCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    BaberId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryAppointments", x => x.TemporaryAppointmentId);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryAppointmentServices",
                columns: table => new
                {
                    TemporaryAppointmentServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporaryAppointmentId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryAppointmentServices", x => x.TemporaryAppointmentServiceId);
                    table.ForeignKey(
                        name: "FK_TemporaryAppointmentServices_TemporaryAppointments_TemporaryAppointmentId",
                        column: x => x.TemporaryAppointmentId,
                        principalTable: "TemporaryAppointments",
                        principalColumn: "TemporaryAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryAppointmentServices_TemporaryAppointmentId",
                table: "TemporaryAppointmentServices",
                column: "TemporaryAppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "PublicSections");

            migrationBuilder.DropTable(
                name: "TemporaryAppointmentServices");

            migrationBuilder.DropTable(
                name: "TemporaryAppointments");
        }
    }
}
