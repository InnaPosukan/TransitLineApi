﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransitLine.DBContext;

#nullable disable

namespace TransitLine.Migrations
{
    [DbContext(typeof(TransitLineContext))]
    [Migration("20231119125333_v5")]
    partial class v5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TransitLine.Models.CargoType", b =>
                {
                    b.Property<int>("IdCargo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_cargo");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCargo"));

                    b.Property<decimal?>("CargoWeight")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("cargo_weight");

                    b.Property<int?>("NumberUnits")
                        .HasColumnType("int")
                        .HasColumnName("numberUnits");

                    b.HasKey("IdCargo")
                        .HasName("PK__Cargo_ty__D3C09EC54444D46A");

                    b.ToTable("Cargo_type", (string)null);
                });

            modelBuilder.Entity("TransitLine.Models.Delivery", b =>
                {
                    b.Property<int>("IdDelivery")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_delivery");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDelivery"));

                    b.Property<string>("DeliveryStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("deliveryStatus");

                    b.Property<DateTime?>("DepartureDate")
                        .HasColumnType("datetime")
                        .HasColumnName("departureDate");

                    b.Property<DateTime?>("DestinationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("destinationDate");

                    b.Property<int?>("IdOrder")
                        .HasColumnType("int")
                        .HasColumnName("id_order");

                    b.HasKey("IdDelivery")
                        .HasName("PK__Delivery__D7513687C607ACD2");

                    b.HasIndex("IdOrder");

                    b.ToTable("Delivery", (string)null);
                });

            modelBuilder.Entity("TransitLine.Models.Order", b =>
                {
                    b.Property<int>("IdOrder")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_order");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrder"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("date")
                        .HasColumnName("creationDate");

                    b.Property<string>("DepartureLocation")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("departureLocation");

                    b.Property<string>("DestinationLocation")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("destinationLocation");

                    b.Property<float>("Distance")
                        .HasColumnType("real");

                    b.Property<int>("DriverUserId")
                        .HasColumnType("int");

                    b.Property<int>("IdCargo")
                        .HasColumnType("int")
                        .HasColumnName("id_cargo");

                    b.Property<int>("IdUser")
                        .HasColumnType("int")
                        .HasColumnName("id_user");

                    b.Property<string>("OrderStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("orderStatus");

                    b.HasKey("IdOrder")
                        .HasName("PK__Orders__DD5B8F3F05113DAA");

                    b.HasIndex("IdCargo");

                    b.HasIndex("IdUser");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TransitLine.Models.Payment", b =>
                {
                    b.Property<int>("IdPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_payment");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPayment"));

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("amount");

                    b.Property<string>("Currency")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("currency");

                    b.Property<int?>("IdOrder")
                        .HasColumnType("int")
                        .HasColumnName("id_order");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("paymentDate");

                    b.Property<string>("PaymentMethod")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("paymentMethod");

                    b.Property<string>("PaymentStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("paymentStatus");

                    b.HasKey("IdPayment")
                        .HasName("PK__Payment__862FEFE02213E7F8");

                    b.HasIndex("IdOrder");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("TransitLine.Models.TemperatureHumidity", b =>
                {
                    b.Property<int>("IdTemperatureHumidity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_temperatureHumidity");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTemperatureHumidity"));

                    b.Property<decimal?>("Humidity")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("humidity");

                    b.Property<int?>("IdDelivery")
                        .HasColumnType("int")
                        .HasColumnName("id_delivery");

                    b.Property<decimal?>("Temperature")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("temperature");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("datetime")
                        .HasColumnName("timestamp");

                    b.HasKey("IdTemperatureHumidity")
                        .HasName("PK__Temperat__E4E7B0A5DC13D7FA");

                    b.HasIndex("IdDelivery");

                    b.ToTable("TemperatureHumidity", (string)null);
                });

            modelBuilder.Entity("TransitLine.Models.Truck", b =>
                {
                    b.Property<int>("IdTruck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_truck");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTruck"));

                    b.Property<decimal?>("Capacity")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("capacity");

                    b.Property<string>("CarNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("car_number");

                    b.Property<decimal?>("Height")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("height");

                    b.Property<decimal?>("Length")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("length");

                    b.Property<string>("Model")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("model");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("IdTruck")
                        .HasName("PK__Truck__780F7FB15AD9F965");

                    b.HasIndex("UserId");

                    b.ToTable("Truck", (string)null);
                });

            modelBuilder.Entity("TransitLine.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_user");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("firstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("lastName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("phoneNumber");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUser")
                        .HasName("PK__Users__D2D14637FE6C42F1");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TransitLine.Models.Delivery", b =>
                {
                    b.HasOne("TransitLine.Models.Order", "IdOrderNavigation")
                        .WithMany("Deliveries")
                        .HasForeignKey("IdOrder")
                        .HasConstraintName("FK__Delivery__id_ord__440B1D61");

                    b.Navigation("IdOrderNavigation");
                });

            modelBuilder.Entity("TransitLine.Models.Order", b =>
                {
                    b.HasOne("TransitLine.Models.CargoType", "IdCargoNavigation")
                        .WithMany("Orders")
                        .HasForeignKey("IdCargo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Orders__id_cargo__3A81B327");

                    b.HasOne("TransitLine.Models.User", "IdUserNavigation")
                        .WithMany("Orders")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Orders__id_user__3B75D760");

                    b.Navigation("IdCargoNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("TransitLine.Models.Payment", b =>
                {
                    b.HasOne("TransitLine.Models.Order", "IdOrderNavigation")
                        .WithMany("Payments")
                        .HasForeignKey("IdOrder")
                        .HasConstraintName("FK__Payment__id_orde__3E52440B");

                    b.Navigation("IdOrderNavigation");
                });

            modelBuilder.Entity("TransitLine.Models.TemperatureHumidity", b =>
                {
                    b.HasOne("TransitLine.Models.Delivery", "IdDeliveryNavigation")
                        .WithMany("TemperatureHumidities")
                        .HasForeignKey("IdDelivery")
                        .HasConstraintName("FK__Temperatu__id_de__46E78A0C");

                    b.Navigation("IdDeliveryNavigation");
                });

            modelBuilder.Entity("TransitLine.Models.Truck", b =>
                {
                    b.HasOne("TransitLine.Models.User", "User")
                        .WithMany("Trucks")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Truck__user_id__412EB0B6");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TransitLine.Models.CargoType", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("TransitLine.Models.Delivery", b =>
                {
                    b.Navigation("TemperatureHumidities");
                });

            modelBuilder.Entity("TransitLine.Models.Order", b =>
                {
                    b.Navigation("Deliveries");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("TransitLine.Models.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Trucks");
                });
#pragma warning restore 612, 618
        }
    }
}