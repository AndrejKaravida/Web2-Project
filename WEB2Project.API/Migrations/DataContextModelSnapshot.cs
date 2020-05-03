﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WEB2Project.API.Data;

namespace WEB2Project.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WEB2Project.API.Models.AircompanyModels.FlightReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ArrivalDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Seats")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TravelLength")
                        .HasColumnType("float");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FlightReservations");
                });

            modelBuilder.Entity("WEB2Project.Dtos.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NeedToChangePassword")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WEB2Project.Models.AirCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<double>("AverageGrade")
                        .HasColumnType("float");

                    b.Property<int?>("HeadOfficeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PromoDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("HeadOfficeId");

                    b.ToTable("AirCompanies");
                });

            modelBuilder.Entity("WEB2Project.Models.Destination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Destinations");
                });

            modelBuilder.Entity("WEB2Project.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirCompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("ArrivalDestinationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("AverageGrade")
                        .HasColumnType("float");

                    b.Property<int?>("DepartureDestinationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Discount")
                        .HasColumnType("bit");

                    b.Property<double>("Mileage")
                        .HasColumnType("float");

                    b.Property<double>("TicketPrice")
                        .HasColumnType("float");

                    b.Property<double>("TravelTime")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AirCompanyId");

                    b.HasIndex("ArrivalDestinationId");

                    b.HasIndex("DepartureDestinationId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("WEB2Project.Models.RentACarCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<double>("AverageGrade")
                        .HasColumnType("float");

                    b.Property<int?>("HeadOfficeId")
                        .HasColumnType("int");

                    b.Property<double>("MonthRentalDiscount")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PromoDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WeekRentalDiscount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("HeadOfficeId");

                    b.ToTable("RentACarCompanies");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MapString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RentACarCompanyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RentACarCompanyId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.CompanyRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RentACarCompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RentACarCompanyId");

                    b.ToTable("CompanyRatings");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Income", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RentACarCompanyId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("RentACarCompanyId");

                    b.ToTable("Income");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DaysLeft")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfDays")
                        .HasColumnType("int");

                    b.Property<string>("ReturningLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StartingLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.ReservedDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("ReservedDate");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.VehicleRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("VehicleRatings");
                });

            modelBuilder.Entity("WEB2Project.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AverageGrade")
                        .HasColumnType("float");

                    b.Property<string>("CurrentDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Doors")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnDiscount")
                        .HasColumnType("bit");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OldPrice")
                        .HasColumnType("int");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("RentACarCompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RentACarCompanyId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("WEB2Project.Models.AirCompany", b =>
                {
                    b.HasOne("WEB2Project.Dtos.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("WEB2Project.Models.RentacarModels.Branch", "HeadOffice")
                        .WithMany()
                        .HasForeignKey("HeadOfficeId");
                });

            modelBuilder.Entity("WEB2Project.Models.Flight", b =>
                {
                    b.HasOne("WEB2Project.Models.AirCompany", null)
                        .WithMany("Flights")
                        .HasForeignKey("AirCompanyId");

                    b.HasOne("WEB2Project.Models.Destination", "ArrivalDestination")
                        .WithMany()
                        .HasForeignKey("ArrivalDestinationId");

                    b.HasOne("WEB2Project.Models.Destination", "DepartureDestination")
                        .WithMany()
                        .HasForeignKey("DepartureDestinationId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentACarCompany", b =>
                {
                    b.HasOne("WEB2Project.Dtos.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("WEB2Project.Models.RentacarModels.Branch", "HeadOffice")
                        .WithMany()
                        .HasForeignKey("HeadOfficeId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Branch", b =>
                {
                    b.HasOne("WEB2Project.Models.RentACarCompany", null)
                        .WithMany("Branches")
                        .HasForeignKey("RentACarCompanyId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.CompanyRating", b =>
                {
                    b.HasOne("WEB2Project.Models.RentACarCompany", null)
                        .WithMany("Ratings")
                        .HasForeignKey("RentACarCompanyId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Income", b =>
                {
                    b.HasOne("WEB2Project.Models.RentACarCompany", null)
                        .WithMany("Incomes")
                        .HasForeignKey("RentACarCompanyId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.Reservation", b =>
                {
                    b.HasOne("WEB2Project.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.ReservedDate", b =>
                {
                    b.HasOne("WEB2Project.Models.Vehicle", null)
                        .WithMany("ReservedDates")
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("WEB2Project.Models.RentacarModels.VehicleRating", b =>
                {
                    b.HasOne("WEB2Project.Models.Vehicle", null)
                        .WithMany("Ratings")
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("WEB2Project.Models.Vehicle", b =>
                {
                    b.HasOne("WEB2Project.Models.RentACarCompany", null)
                        .WithMany("Vehicles")
                        .HasForeignKey("RentACarCompanyId");
                });
#pragma warning restore 612, 618
        }
    }
}
