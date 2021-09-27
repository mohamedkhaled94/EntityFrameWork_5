using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppDataFirstLook")
                .LogTo(Console.WriteLine , new[] { DbLoggerCategory.Database.Command.Name}, Microsoft.Extensions.Logging.LogLevel.Information )
                .EnableSensitiveDataLogging();
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(sp => sp.Samurais)
                .UsingEntity<SamuraiBattle>
                (b => b.HasOne<Battle>().WithMany(), b => b.HasOne<Samurai>().WithMany())
                .Property(b => b.DateJoined)
                .HasDefaultValueSql("getdate()");
        }
    }
}
