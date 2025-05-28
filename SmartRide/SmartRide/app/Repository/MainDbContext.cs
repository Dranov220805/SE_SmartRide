using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace Repository
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Fully qualify ConfigurationManager to resolve ambiguity
        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new Exception("Connection string 'DefaultConnection' is missing or empty. Check App.config!");
        //    }

        //    optionsBuilder.UseSqlServer(connectionString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ride: Pickup and Dropoff Locations
            //modelBuilder.Entity<Ride>()
            //    .HasMany(r => r.Locations)
            //    .WithMany(l => l.Rides);

            modelBuilder.Entity<Ride>()
                .HasOne(r => r.PickupLocation)
                .WithMany()
                .HasForeignKey(r => r.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Ride>()
                .HasOne(r => r.DropoffLocation)
                .WithMany()
                .HasForeignKey(r => r.DropoffLocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Payment: Ride
            modelBuilder.Entity<Payment>()
                .HasOne<Ride>()
                .WithMany()
                .HasForeignKey(p => p.RideId)
                .OnDelete(DeleteBehavior.Cascade);

            // Report: Manager
            modelBuilder.Entity<Report>()
                .HasOne<Manager>()
                .WithMany()
                .HasForeignKey(r => r.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithMany()
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
