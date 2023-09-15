using Microsoft.EntityFrameworkCore;
using OcrInvoiceBackend.Domain.Entities;

namespace OcrInvoiceBackend.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Statistics> Statistics { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>(invoice =>
            {
                invoice.HasOne(i => i.Scan)
                    .WithOne(s => s.Invoice)
                    .HasForeignKey<ScanResults>(s => s.InvoiceId);

                invoice.HasMany(i => i.Details)
                    .WithOne(s => s.Invoice)
                    .HasForeignKey(s => s.InvoiceId);
            });

            modelBuilder.Entity<ScanResults>(scan =>
            {
                scan.OwnsOne(s => s.ImageBounds);
                scan.OwnsOne(s => s.ContentBounds);
            });
        }
        #endregion
    }
}
