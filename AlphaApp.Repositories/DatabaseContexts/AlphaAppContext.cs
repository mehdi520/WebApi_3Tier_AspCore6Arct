using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AlphaApp.Repositories.Entities;

namespace AlphaApp.Repositories.DatabaseContexts
{
    public partial class AlphaAppContext : DbContext
    {
        public AlphaAppContext()
        {
        }

        public AlphaAppContext(DbContextOptions<AlphaAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UAddress> UAddresses { get; set; } = null!;
        public virtual DbSet<UCountry> UCountries { get; set; } = null!;
        public virtual DbSet<UUser> UUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-45PMD0M\\MSSQLSERVER19;Database=AlphaAppDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.ToTable("U_Address");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<UCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("U_Country");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<UUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("U_User");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
