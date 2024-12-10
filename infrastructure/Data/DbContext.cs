
using Barbara.Core.Entities;
using CMPNatural.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Entities;
using ScoutDirect.Core.Entities.Base;

namespace infrastructure.Data
{
    public partial class ScoutDBContext : DbContext
    {
        public ScoutDBContext()
        {
        }

        public ScoutDBContext(DbContextOptions<ScoutDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Company> Company { get; set; } = null!;
        public virtual DbSet<LocationCompany> LocationCompany { get; set; } = null!;
        public virtual DbSet<DocumentSubmission> DocumentSubmission { get; set; } = null!;
        public virtual DbSet<BillingInformation> BillingInformation { get; set; } = null!;
        public virtual DbSet<BusinessType> BusinessType { get; set; } = null!;
        public virtual DbSet<OperationalAddress> OperationalAddress { get; set; } = null!;
        public virtual DbSet<BaseServiceAppointment> BaseServiceAppointment { get; set; } = null!;
        public virtual DbSet<ServiceAppointment> ServiceAppointment { get; set; } = null!;
        public virtual DbSet<ServiceAppointmentEmergency> ServiceAppointmentEmergency { get; set; } = null!;
        public virtual DbSet<Invoice> Invoice { get; set; } = null!;
        public virtual DbSet<ShoppingCard> ShoppingCard { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=188.245.68.131;Port=3306;Database=ScoutDirect;User Id=sammy;Password=Sdsw#2a1@=7632;"

        );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");
            });

            modelBuilder.Entity<LocationCompany>(entity =>
            {
                entity.ToTable("LocationCompany");
            });

            modelBuilder.Entity<DocumentSubmission>(entity =>
            {
                entity.ToTable("DocumentSubmission");
            });

            modelBuilder.Entity<BillingInformation>(entity =>
            {
                entity.ToTable("BillingInformation");
            });

            modelBuilder.Entity<OperationalAddress>(entity =>
            {
                entity.ToTable("OperationalAddress");
                entity.HasMany(d => d.LocationCompany)
                .WithOne(p => p.OperationalAddress)
                .HasForeignKey(d => d.OperationalAddressId);
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.ToTable("BusinessType");
            });

            modelBuilder.Entity<BaseServiceAppointment>(entity =>
            {
                entity.ToTable("BaseServiceAppointment");

                entity.HasMany(sa => sa.ServiceAppointmentLocations)
                .WithOne(sal => sal.ServiceAppointment)
                .HasForeignKey(sal => sal.ServiceAppointmentId);
            });

            modelBuilder.Entity<ServiceAppointment>(entity =>
            {
                entity.ToTable("ServiceAppointment").HasBaseType<BaseServiceAppointment>();
                entity.Property(e => e.ServiceCrmId)
                  .HasColumnType("varchar(255)") 
                  .IsRequired(false);



            });
            modelBuilder.Entity<ServiceAppointmentEmergency>(entity =>
            {
                entity.ToTable("ServiceAppointmentEmergency").HasBaseType<BaseServiceAppointment>();

            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");
                entity.HasMany(d => d.BaseServiceAppointment)
                      .WithOne(p => p.Invoice)
                 .HasForeignKey(d => d.InvoiceId);
            });

            modelBuilder.Entity<ShoppingCard>(entity =>
            {
                entity.ToTable("ShoppingCard");
            });

            modelBuilder.Entity<ServiceAppointmentLocation>(entity =>
            {
                entity.ToTable("ServiceAppointmentLocation");
            });

        }

    }

}