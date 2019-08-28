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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(u =>
            {
                u.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                u.Property(p => p.Email)
                    .HasMaxLength(50)
                    .IsRequired();

                u.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                u.Property(p => p.Password)
                    .HasMaxLength(500)
                    .IsRequired();

                u.Property(p => p.DateRegister)
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("(NOW())");

                u.Property(p => p.Status)
                    .HasColumnType("BIT")
                    .HasDefaultValueSql("(1)");

                u.HasKey(p => p.Id);

                u.ToTable("Users");
            });

            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired();


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
