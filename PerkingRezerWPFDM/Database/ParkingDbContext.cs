using Microsoft.EntityFrameworkCore;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.Database
{
    public class ParkingDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=parking.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ustawienie klucza głównego
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            // Ustawienie unikalności dla Username (nie można mieć dwóch takich samych loginów)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Domyślna wartość dla roli użytkownika
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Użytkownik");

            // Pole PasswordHash nie może być null
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
