using Booking.System.PaymentService.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.System.PaymentService.DataBase.Context;

public class PaymentContext(DbContextOptions<PaymentContext> options) : DbContext(options)
{
    public DbSet<DbPayment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPayment>(entity =>
        {
            entity.ToTable("payments"); 

            entity.Property(p => p.Id)
                .HasColumnName("id"); 
            
            entity.Property(p => p.PaymentUid)
                .HasColumnName("payment_uid");
            
            entity.Property(p => p.PaymentStatus)
                .HasConversion<string>()
                .HasDefaultValue("PAID")
                .HasColumnName("status");
            
            entity.Property(p => p.Price)
                .HasColumnName("price");
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
