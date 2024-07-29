
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
        //public virtual DbSet<Person> Person { get; set; } = null!;
        //public virtual DbSet<ScoutEntity> Scout { get; set; } = null!;
        //public virtual DbSet<Appointment> Appointment { get; set; } = null!;
        //public virtual DbSet<Report> Report { get; set; } = null!;
        //public virtual DbSet<LocationScout> LocationScout { get; set; } = null!;

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
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.ToTable("BusinessType");
            });




        }

    }

}