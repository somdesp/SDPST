using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<Country>().HasKey(p => p.Id);
            modelBuilder.Entity<State>().HasKey(p => p.Id);
            modelBuilder.Entity<City>().HasKey(p => p.Id);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Country>().ToTable("Countries");

            modelBuilder.Entity<State>().ToTable("States")           
                    .HasOne(p => p.Country)
                    .WithMany(b => b.States)
                    .HasForeignKey(p => p.CountryId)
                    .HasConstraintName("ForeignKey_Country_States");

            modelBuilder.Entity<City>().ToTable("Cities")
                .HasOne(p => p.State)
                .WithMany(b => b.Cities)
                .HasForeignKey(p => p.StateId)
                .HasConstraintName("ForeignKey_States_City");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
