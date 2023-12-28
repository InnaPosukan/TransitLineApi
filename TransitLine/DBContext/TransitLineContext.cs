using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransitLine.Models;

namespace TransitLine.DBContext;

public partial class TransitLineContext : DbContext
{
    private readonly IConfiguration _configuration;

    public TransitLineContext(DbContextOptions<TransitLineContext> options,IConfiguration configuration )
    : base(options)

    {
        _configuration = configuration;

    }

    public virtual DbSet<CargoType> CargoTypes { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<TemperatureHumidity> TemperatureHumidities { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CargoType>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargo_ty__D3C09EC54444D46A");

            entity.ToTable("Cargo_type");

            entity.Property(e => e.IdCargo).HasColumnName("id_cargo");
            entity.Property(e => e.CargoWeight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cargo_weight");
            entity.Property(e => e.NumberUnits).HasColumnName("numberUnits");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.IdDelivery).HasName("PK__Delivery__D7513687C607ACD2");

            entity.ToTable("Delivery");

            entity.Property(e => e.IdDelivery).HasColumnName("id_delivery");
            entity.Property(e => e.DeliveryStatus)
                .HasMaxLength(50)
                .HasColumnName("deliveryStatus");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("datetime")
                .HasColumnName("departureDate");
            entity.Property(e => e.DestinationDate)
                .HasColumnType("datetime")
                .HasColumnName("destinationDate");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK__Delivery__id_ord__440B1D61");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__DD5B8F3F05113DAA");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.DepartureLocation)
                .HasMaxLength(100)
                .HasColumnName("departureLocation");
            entity.Property(e => e.DestinationLocation)
                .HasMaxLength(100)
                .HasColumnName("destinationLocation");
            entity.Property(e => e.IdCargo).HasColumnName("id_cargo");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasColumnName("orderStatus");

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCargo)
                .HasConstraintName("FK__Orders__id_cargo__3A81B327");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Orders__id_user__3B75D760");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("PK__Payment__862FEFE02213E7F8");

            entity.ToTable("Payment");

            entity.Property(e => e.IdPayment).HasColumnName("id_payment");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasColumnName("currency");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("paymentStatus");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK__Payment__id_orde__3E52440B");
        });

        modelBuilder.Entity<TemperatureHumidity>(entity =>
        {
            entity.HasKey(e => e.IdTemperatureHumidity).HasName("PK__Temperat__E4E7B0A5DC13D7FA");

            entity.ToTable("TemperatureHumidity");

            entity.Property(e => e.IdTemperatureHumidity).HasColumnName("id_temperatureHumidity");
            entity.Property(e => e.Humidity)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("humidity");
            entity.Property(e => e.IdDelivery).HasColumnName("id_delivery");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("temperature");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.IdDeliveryNavigation).WithMany(p => p.TemperatureHumidities)
                .HasForeignKey(d => d.IdDelivery)
                .HasConstraintName("FK__Temperatu__id_de__46E78A0C");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(e => e.IdTruck).HasName("PK__Truck__780F7FB15AD9F965");

            entity.ToTable("Truck");

            entity.Property(e => e.IdTruck).HasColumnName("id_truck");
            entity.Property(e => e.Capacity)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("capacity");
            entity.Property(e => e.CarNumber)
                .HasMaxLength(15)
                .HasColumnName("car_number");
            entity.Property(e => e.Height)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("height");
            entity.Property(e => e.Length)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("length");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .HasColumnName("model");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Trucks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Truck__user_id__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__D2D14637FE6C42F1");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnName("phoneNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
