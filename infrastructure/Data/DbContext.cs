
using Barbara.Core.Entities;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public virtual DbSet<Menu> Menu { get; set; } = null!;
        public virtual DbSet<AdminMenuAccess> AdminMenuAccess { get; set; } = null!;
        public virtual DbSet<Contract> Contract { get; set; } = null!;
        public virtual DbSet<AppInformation> AppInformation { get; set; } = null!;
        public virtual DbSet<TermsConditions> TermsConditions { get; set; } = null!;
        public virtual DbSet<Manifest> Manifest { get; set; } = null!;
        public virtual DbSet<BillingInformationProvider> BillingInformationProvider { get; set; } = null!;
        public virtual DbSet<ServiceArea> ServiceArea { get; set; } = null!;
        public virtual DbSet<Payment> Payment { get; set; } = null!;
        public virtual DbSet<AppLog> AppLog { get; set; } = null!;
        public virtual DbSet<GoHighLevel> GoHighLevel { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //// Build config from appsettings.json
                //var config = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory()) // Important for locating the file
                //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //    .Build();

                //var connectionString = config.GetConnectionString("LocalConnection");

                optionsBuilder.UseMySQL("Server=64.20.208.14;Port=3306;Database=CmpAppLocal;User Id=erfan;Password=5ssdf@hFghj227sdfjhFDdnsjgGgyR&;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoHighLevel>(entity =>
            {
                entity.ToTable("GoHighLevel");
            });

            modelBuilder.Entity<AppLog>(entity =>
            {
                entity.ToTable("AppLog");
                entity.Property(p => p.LogType)
                .HasConversion(
                 x => x.ToString(),
                 x => (LogTypeEnum)Enum.Parse(typeof(LogTypeEnum), x)
                 );
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.Property(d => d.Status)
                 .HasConversion(
                 x => x.ToString(),
                 x => (PaymentHistoryStatus)Enum.Parse(typeof(PaymentHistoryStatus), x)
                 );
            });

            modelBuilder.Entity<ServiceArea>(entity =>
            {
                entity.ToTable("ServiceArea");
                entity.Property(d => d.ServiceAreaType)
                    .HasConversion(
                    x => x.ToString(),
                    x => (ServiceAreaTypeEnum)Enum.Parse(typeof(ServiceAreaTypeEnum), x)
                    );
            });

            modelBuilder.Entity<BillingInformationProvider>(entity =>
            {
                entity.ToTable("BillingInformationProvider");
            });

            modelBuilder.Entity<Manifest>(entity =>
            {
                entity.ToTable("Manifest");

                entity.Property(p => p.Status)
                 .HasConversion(
                  x => x.ToString(),
                  x => (ManifestStatus)Enum.Parse(typeof(ManifestStatus), x)
                  );
                entity.HasOne(d => d.Invoice)
                .WithOne()
                .HasForeignKey<Manifest>(d => d.InvoiceId);

                entity.HasOne(d => d.Provider)
                .WithOne()
                .HasForeignKey<Manifest>(d => d.ProviderId);
            });

            modelBuilder.Entity<TermsConditions>(entity =>
            {
                entity.ToTable("TermsConditions");
                entity.Property(p => p.Type)
                .HasConversion(
                 x => x.ToString(),
                 x => (TermsConditionsEnum)Enum.Parse(typeof(TermsConditionsEnum), x)
                 );
            });

            modelBuilder.Entity<AppInformation>(entity =>
            {
                entity.ToTable("AppInformation");
            });

            modelBuilder.Entity<InvoiceSource>(entity =>
            {
                entity.ToTable("InvoiceSource");
            });

            modelBuilder.Entity<CompanyContract>(entity =>
            {
                entity.ToTable("CompanyContract");

                entity.HasOne(d => d.Company)
                .WithMany(x=>x.CompanyContract)
                .HasForeignKey(d => d.CompanyId);

                entity.Property(p => p.Status)
                .HasConversion(
                 x => x.ToString(),
                 x => (CompanyContractStatus)Enum.Parse(typeof(CompanyContractStatus), x)
                 );
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");
            });

            modelBuilder.Entity<AdminMenuAccess>(entity =>
            {
                entity.ToTable("AdminMenuAccess");
                entity.HasOne(d => d.Menu)
                .WithOne()
                .HasForeignKey<AdminMenuAccess>(d => d.MenuId);
            });

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
                entity.Property(d => d.VehicleServiceStatus)
                 .HasConversion(
                 x => x.ToString(),
                 x => (VehicleServiceStatus)Enum.Parse(typeof(VehicleServiceStatus), x)
                 );
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

                entity.Property(d => d.Status)
                .HasConversion(
                x => x.ToString(),
                x => (CompanyStatus)Enum.Parse(typeof(CompanyStatus), x)
                );

                entity.HasOne(d => d.BillingInformation)
                .WithOne(p => p.Company)
                .HasForeignKey<BillingInformation>(d => d.CompanyId);
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


                entity.Property(d => d.Status)
                .HasConversion(
                 x => x.ToString(),
                 x => (ServiceStatus)Enum.Parse(typeof(ServiceStatus), x)
                 );

                entity.Property(d => d.OilQuality)
                  .HasConversion(
                    v => v != null ? v.ToString() : null,
                    v => v != null ? (OilQualityEnum)Enum.Parse(typeof(OilQualityEnum), v) : (OilQualityEnum?)null
                  );

                entity.Property(p => p.CancelBy)
                .HasConversion(
                 x => x.ToString(),
                 x => x == null ? null : (CancelEnum)Enum.Parse(typeof(CancelEnum), x)
                 );

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

                entity
                .Property(p => p.Status)
                .HasConversion(
                x => x.ToString(),
                x => (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus), x)
                );

                entity.Property(d => d.PaymentStatus)
                .HasConversion(
                 x => x.ToString(),
                 x => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), x)
                 );

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

                entity.HasOne(d => d.Person)
                .WithOne()
                .HasForeignKey<AdminEntity>(d => d.PersonId);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Provider");

                entity
                .Property(p => p.Status)
                .HasConversion<int>();

                entity
              .Property(p => p.RegistrationStatus)
                .HasConversion(
                  x => x.ToString(),
                   x => (ProviderRegistrationStatus)Enum.Parse(typeof(ProviderRegistrationStatus), x)
                  );

                entity.HasMany(d => d.ProviderService)
                .WithOne(p => p.Provider)
.               HasForeignKey(d => d.ProviderId);

                entity.HasMany(d => d.ServiceArea)
                .WithOne(p => p.Provider)
                .HasForeignKey(d => d.ProviderId);
            });

            modelBuilder.Entity<Capacity>(entity =>
            {
                entity.ToTable("Capacity");
            });

            modelBuilder.Entity<ProviderService>(entity =>
            {
             entity.ToTable("ProviderService");

             entity.HasOne(d => d.Product)
            .WithMany(p => p.ProviderService)
            .HasForeignKey(d => d.ProductId);

             entity.HasOne(d => d.Provider)
            .WithMany(p => p.ProviderService)
            .HasForeignKey(d => d.ProviderId);
            });
        }

    }

}