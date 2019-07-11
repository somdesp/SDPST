using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Country>().ToTable("Country");

            modelBuilder.Entity<State>().ToTable("State")           
                    .HasOne(p => p.Country)
                    .WithMany(b => b.States)
                    .HasForeignKey(p => p.CountryId)
                    .HasConstraintName("ForeignKey_Country_States");

            modelBuilder.Entity<City>().ToTable("City")
                .HasOne(p => p.State)
                .WithMany(b => b.Cities)
                .HasForeignKey(p => p.StateId)
                .HasConstraintName("ForeignKey_States_City");

        }
    }
}
