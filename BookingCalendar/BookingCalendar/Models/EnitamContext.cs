using BookingCalendar.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace BookingCalendar.Models
{
	public class EnitamContext:DbContext
	{
        public EnitamContext(DbContextOptions<EnitamContext> options)
            : base(options)
        {
          
        }
        public EnitamContext()
        {
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("EnitamDB");
            optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Kalendar>()
           //.HasOne(p => p.Evening);
           

           // modelBuilder.Entity<Kalendar>()
           //.Navigation(b => b.Evening)
           //.UsePropertyAccessMode(PropertyAccessMode.Property);
        }
        public DbSet<Login> Login { get; set; } = null!;
        public DbSet<Kalendar> Kalendar { get; set; } = null!;
        public DbSet<Event> Event { get; set; } = null!;
    }
}

