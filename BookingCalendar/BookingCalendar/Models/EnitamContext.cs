using BookingCalendar.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        public DbSet<Login> Login { get; set; } = null!;
        public DbSet<Kalendar> Kalendar { get; set; } = null!;
    }
}

