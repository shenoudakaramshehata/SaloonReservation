﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SaloonReservation.Data;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    [DbContext(typeof(SalonContext))]
    [Migration("20230907145702_Models")]
    partial class Models
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SaloonReservation.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime?>("AppointmentCreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("AppointmentEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("AppointmentStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AppointmentStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("BaberId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("TotalAmount")
                        .HasColumnType("real");

                    b.HasKey("AppointmentId");

                    b.HasIndex("AppointmentStatusId");

                    b.HasIndex("BaberId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("SaloonReservation.Models.AppointmentService", b =>
                {
                    b.Property<int>("AppointmentServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentServiceId"));

                    b.Property<float?>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("AppointmentServiceId");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("GenderId");

                    b.HasIndex("ServiceId");

                    b.ToTable("AppointmentServices");
                });

            modelBuilder.Entity("SaloonReservation.Models.AppointmentStatus", b =>
                {
                    b.Property<int>("AppointmentStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentStatusId"));

                    b.Property<string>("AppointmentStatusTitleAR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppointmentStatusTitleEN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AppointmentStatusId");

                    b.ToTable("AppointmentStatuses");

                    b.HasData(
                        new
                        {
                            AppointmentStatusId = 1,
                            AppointmentStatusTitleAR = "جديد",
                            AppointmentStatusTitleEN = "New"
                        },
                        new
                        {
                            AppointmentStatusId = 2,
                            AppointmentStatusTitleAR = "مغلق",
                            AppointmentStatusTitleEN = "Closed"
                        },
                        new
                        {
                            AppointmentStatusId = 3,
                            AppointmentStatusTitleAR = "ملغي",
                            AppointmentStatusTitleEN = "Canceled"
                        });
                });

            modelBuilder.Entity("SaloonReservation.Models.Area", b =>
                {
                    b.Property<int>("AreaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AreaId"));

                    b.Property<string>("AreaTLAr")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AreaTLEn")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.HasKey("AreaId");

                    b.HasIndex("CityId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("SaloonReservation.Models.Barber", b =>
                {
                    b.Property<int>("BarberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BarberId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("OffWeekDayId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BarberId");

                    b.HasIndex("OffWeekDayId");

                    b.ToTable("Barbers");

                    b.HasData(
                        new
                        {
                            BarberId = 1,
                            Description = "good barber good barber good barber good barber good barber good barber",
                            FullName = "Barber 1",
                            Image = "1.jpg",
                            IsActive = true,
                            Phone = "258745874"
                        },
                        new
                        {
                            BarberId = 2,
                            Description = "good barber good barber good barber good barber good barber good barber",
                            FullName = "Barber 2",
                            Image = "2.jpg",
                            IsActive = true,
                            Phone = "685784845"
                        });
                });

            modelBuilder.Entity("SaloonReservation.Models.BarberImage", b =>
                {
                    b.Property<int>("BarberImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BarberImageId"));

                    b.Property<int>("BarberId")
                        .HasColumnType("int");

                    b.Property<string>("pic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("picDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BarberImageId");

                    b.HasIndex("BarberId");

                    b.ToTable("BarberImages");
                });

            modelBuilder.Entity("SaloonReservation.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("CityTLAr")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CityTLEn")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SaloonReservation.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<int?>("AreaId")
                        .HasColumnType("int");

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MapLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.HasIndex("AreaId");

                    b.HasIndex("CityId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerId = 1,
                            CountryId = 0,
                            Email = "mail@site.com",
                            FullAddress = "Kuwait -sharq - block 2, st 133",
                            FullName = "Customer 1",
                            Phone = "5587485778"
                        });
                });

            modelBuilder.Entity("SaloonReservation.Models.Gender", b =>
                {
                    b.Property<int>("GenderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenderId"));

                    b.Property<string>("GenderTLAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GenderTLEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenderId");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            GenderId = 1,
                            GenderTLAr = "ولد",
                            GenderTLEn = "Boy"
                        },
                        new
                        {
                            GenderId = 2,
                            GenderTLAr = "بنت",
                            GenderTLEn = "Girl"
                        });
                });

            modelBuilder.Entity("SaloonReservation.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceId"));

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<double>("MoreKidsPrice")
                        .HasColumnType("float");

                    b.Property<double>("OneKidPrice")
                        .HasColumnType("float");

                    b.Property<string>("serviceTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("serviceTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceId");

                    b.HasIndex("GenderId");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            ServiceId = 1,
                            GenderId = 1,
                            MoreKidsPrice = 6.0,
                            OneKidPrice = 4.0,
                            serviceTlAr = "قص الشعر",
                            serviceTlEn = "Hair Cut"
                        },
                        new
                        {
                            ServiceId = 2,
                            GenderId = 1,
                            MoreKidsPrice = 6.0,
                            OneKidPrice = 4.0,
                            serviceTlAr = "تقليم الشعر",
                            serviceTlEn = "Hair Trim"
                        },
                        new
                        {
                            ServiceId = 3,
                            GenderId = 1,
                            MoreKidsPrice = 4.0,
                            OneKidPrice = 2.0,
                            serviceTlAr = "تقليم الأمام",
                            serviceTlEn = "Bang Trim"
                        },
                        new
                        {
                            ServiceId = 4,
                            GenderId = 1,
                            MoreKidsPrice = 6.0,
                            OneKidPrice = 4.0,
                            serviceTlAr = "تصفيف الشعر - قصير",
                            serviceTlEn = "Blowing-Short"
                        },
                        new
                        {
                            ServiceId = 5,
                            GenderId = 1,
                            MoreKidsPrice = 8.0,
                            OneKidPrice = 6.0,
                            serviceTlAr = "تصفيف الشعر - متوسط",
                            serviceTlEn = "Blowing-Medium"
                        },
                        new
                        {
                            ServiceId = 6,
                            GenderId = 1,
                            MoreKidsPrice = 10.0,
                            OneKidPrice = 8.0,
                            serviceTlAr = "تصفيف الشعر - طويل",
                            serviceTlEn = "Blowing-Long"
                        },
                        new
                        {
                            ServiceId = 7,
                            GenderId = 1,
                            MoreKidsPrice = 6.0,
                            OneKidPrice = 6.0,
                            serviceTlAr = "تسريح الشعر بالمكواة - قصير",
                            serviceTlEn = "Hair-Iron-Short"
                        },
                        new
                        {
                            ServiceId = 8,
                            GenderId = 1,
                            MoreKidsPrice = 8.0,
                            OneKidPrice = 8.0,
                            serviceTlAr = "تسريح الشعر بالمكواة - متوسط",
                            serviceTlEn = "Hair-Iron-Medium"
                        },
                        new
                        {
                            ServiceId = 9,
                            GenderId = 1,
                            MoreKidsPrice = 10.0,
                            OneKidPrice = 10.0,
                            serviceTlAr = "تسريح الشعر بالمكواة - طويل",
                            serviceTlEn = "Hair-Iron-Long"
                        },
                        new
                        {
                            ServiceId = 10,
                            GenderId = 1,
                            MoreKidsPrice = 1.0,
                            OneKidPrice = 1.0,
                            serviceTlAr = "غسيل الشعر",
                            serviceTlEn = "Hair Wash"
                        },
                        new
                        {
                            ServiceId = 11,
                            GenderId = 1,
                            MoreKidsPrice = 5.0,
                            OneKidPrice = 5.0,
                            serviceTlAr = "علاج الشعر بالزيوت الطبيعية",
                            serviceTlEn = "Hair Treatment With Natural Oils"
                        },
                        new
                        {
                            ServiceId = 12,
                            GenderId = 2,
                            MoreKidsPrice = 6.0,
                            OneKidPrice = 4.0,
                            serviceTlAr = "قص الشعر",
                            serviceTlEn = "Hair Cut"
                        },
                        new
                        {
                            ServiceId = 13,
                            GenderId = 2,
                            MoreKidsPrice = 1.0,
                            OneKidPrice = 1.0,
                            serviceTlAr = "غسيل الشعر",
                            serviceTlEn = "Hair Wash"
                        },
                        new
                        {
                            ServiceId = 14,
                            GenderId = 2,
                            MoreKidsPrice = 4.0,
                            OneKidPrice = 4.0,
                            serviceTlAr = "قص الشعر للحالات الخاصة",
                            serviceTlEn = "Special Needs Hair Cut"
                        },
                        new
                        {
                            ServiceId = 15,
                            GenderId = 2,
                            MoreKidsPrice = 2.0,
                            OneKidPrice = 2.0,
                            serviceTlAr = "حلاقة الكبار",
                            serviceTlEn = "Adult Shaving"
                        });
                });

            modelBuilder.Entity("SaloonReservation.Models.WeekDay", b =>
                {
                    b.Property<int>("WeekDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WeekDayId"));

                    b.Property<int?>("WeekDayIndex")
                        .HasColumnType("int");

                    b.Property<string>("WeekDayTitle")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("WeekDayId");

                    b.ToTable("WeekDays");
                });

            modelBuilder.Entity("SaloonReservation.Models.Appointment", b =>
                {
                    b.HasOne("SaloonReservation.Models.AppointmentStatus", "AppointmentStatus")
                        .WithMany()
                        .HasForeignKey("AppointmentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SaloonReservation.Models.Barber", "Barber")
                        .WithMany("appoitments")
                        .HasForeignKey("BaberId");

                    b.HasOne("SaloonReservation.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppointmentStatus");

                    b.Navigation("Barber");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("SaloonReservation.Models.AppointmentService", b =>
                {
                    b.HasOne("SaloonReservation.Models.Appointment", "Appointment")
                        .WithMany("Services")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SaloonReservation.Models.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId");

                    b.HasOne("SaloonReservation.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Gender");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("SaloonReservation.Models.Area", b =>
                {
                    b.HasOne("SaloonReservation.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("SaloonReservation.Models.Barber", b =>
                {
                    b.HasOne("SaloonReservation.Models.WeekDay", "OffWeekDay")
                        .WithMany()
                        .HasForeignKey("OffWeekDayId");

                    b.Navigation("OffWeekDay");
                });

            modelBuilder.Entity("SaloonReservation.Models.BarberImage", b =>
                {
                    b.HasOne("SaloonReservation.Models.Barber", "Barber")
                        .WithMany("BarberImages")
                        .HasForeignKey("BarberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Barber");
                });

            modelBuilder.Entity("SaloonReservation.Models.Customer", b =>
                {
                    b.HasOne("SaloonReservation.Models.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId");

                    b.HasOne("SaloonReservation.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.Navigation("Area");

                    b.Navigation("City");
                });

            modelBuilder.Entity("SaloonReservation.Models.Service", b =>
                {
                    b.HasOne("SaloonReservation.Models.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("SaloonReservation.Models.Appointment", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("SaloonReservation.Models.Barber", b =>
                {
                    b.Navigation("BarberImages");

                    b.Navigation("appoitments");
                });
#pragma warning restore 612, 618
        }
    }
}
