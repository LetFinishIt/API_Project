using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FinalProjectLibrary.Models
{
    public partial class FinalProjectDBContext : DbContext
    {
        public FinalProjectDBContext()
        {
        }

        public FinalProjectDBContext(DbContextOptions<FinalProjectDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingSlot> BookingSlots { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<UserCred> UserCreds { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=mssqllab.chsotyho5gzz.us-east-1.rds.amazonaws.com;database=FinalProjectDB;User ID=phongvu;Password=Boombayah99;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.HasOne(d => d.BookingSlot)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingSlotId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<BookingSlot>(entity =>
            {
                entity.ToTable("BookingSlot");

                entity.Property(e => e.SlotTime).HasColumnType("datetime");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.BookingSlots)
                    .HasForeignKey(d => d.RestaurantId);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant");

                entity.Property(e => e.CuisineType).HasMaxLength(50);

                entity.Property(e => e.RestaurantAddress).HasMaxLength(500);

                entity.Property(e => e.RestaurantCity).HasMaxLength(50);

                entity.Property(e => e.RestaurantName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.OwnerUser)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UserCred>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserCred__1788CC4C902B9BCE");

                entity.ToTable("UserCred");

                entity.HasIndex(e => e.Email, "UQ__UserCred__A9D105340B1CB90A")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
