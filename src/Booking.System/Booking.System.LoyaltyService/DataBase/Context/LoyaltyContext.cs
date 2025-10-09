using Microsoft.EntityFrameworkCore;

namespace Booking.System.LoyaltyService.DataBase.Context;


public class LoyaltyContext(DbContextOptions<LoyaltyContext> options) : DbContext(options)
{
    public DbSet<> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
    }
}