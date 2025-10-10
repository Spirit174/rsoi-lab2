using Booking.System.PaymentService.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.System.PaymentService.DataBase.Context;

public class PaymentContext(DbContextOptions<PaymentContext> options) : DbContext(options)
{
    public DbSet<DbPayment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}