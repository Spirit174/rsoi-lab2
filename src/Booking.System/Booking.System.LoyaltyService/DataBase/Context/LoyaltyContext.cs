using Booking.System.LoyaltyService.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.System.LoyaltyService.DataBase.Context;


public class LoyaltyContext(DbContextOptions<LoyaltyContext> options) : DbContext(options)
{
    public DbSet<DbLoyalty> Loyalties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LoyaltyConfiguration());
    }
}