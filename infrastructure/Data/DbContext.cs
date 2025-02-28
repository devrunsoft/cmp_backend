
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
        public virtual DbSet<AdminEntity> Admin { get; set; } = null!;
        public virtual DbSet<Person> Person { get; set; } = null!;
        public virtual DbSet<Provider> Provider { get; set; } = null!;
        public virtual DbSet<ProviderServiceAssignment> ProviderServiceAssignment { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<ProductPrice> ProductPrice { get; set; } = null!;
        public virtual DbSet<InvoiceProduct> InvoiceProduct { get; set; } = null!;
        public virtual DbSet<Driver> Driver { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicle { get; set; } = null!;
        public virtual DbSet<VehicleCompartment> VehicleCompartment { get; set; } = null!;
        public virtual DbSet<VehicleService> VehicleService { get; set; } = null!;

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
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");
            });

            modelBuilder.Entity<VehicleCompartment>(entity =>
            {
                entity.ToTable("VehicleCompartment");
            });

            modelBuilder.Entity<VehicleService>(entity =>
            {
                entity.ToTable("VehicleService");
            });

            modelBuilder.Entity<InvoiceProduct>(entity =>
            {
                entity.ToTable("InvoiceProduct");

                entity.HasOne(d => d.ProductPrice)
                .WithMany(p => p.InvoiceProduct)
                .HasForeignKey(d => d.ProductPriceId);

                entity.HasOne(d => d.Invoice)
                .WithMany(p => p.InvoiceProduct)
                .HasForeignKey(d => d.InvoiceId);
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.ToTable("ProductPrice");

                entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductPrice)
                .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");
            });

            modelBuilder.Entity<LocationCompany>(entity =>
            {
                entity.ToTable("LocationCompany");
                entity.HasOne(d => d.CapacityEntity)
                .WithMany(p => p.LocationCompany)
                .HasForeignKey(d => d.CapacityId);
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

                entity.HasOne(sa => sa.Product)
                .WithMany(sal => sal.ServiceAppointment)
                .HasForeignKey(sal => sal.ProductId);

                entity.HasOne(sa => sa.ProductPrice)
               .WithMany(sal => sal.ServiceAppointment)
               .HasForeignKey(sal => sal.ProductPriceId);

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

                entity.HasOne(d => d.Company)
                      .WithMany(p => p.Invoices)
                 .HasForeignKey(d => d.CompanyId);

                entity.HasMany(d => d.InvoiceProduct)
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

                entity.HasOne(d => d.LocationCompany)
                .WithMany(p => p.ServiceAppointmentLocations)
                .HasForeignKey(d => d.LocationCompanyId);

            });


            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
            });

            modelBuilder.Entity<AdminEntity>(entity =>
            {
                entity.ToTable("Admin");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Provider");
            });

            modelBuilder.Entity<Capacity>(entity =>
            {
                entity.ToTable("Capacity");
            });
        }

    }

}