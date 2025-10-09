using Booking.System.LoyaltyService.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.System.LoyaltyService.DataBase.Context;

public class LoyaltyConfiguration : IEntityTypeConfiguration<DbLoyalty>
{
    public void Configure(EntityTypeBuilder<DbLoyalty> builder)
    {
        builder.HasIndex(loyalty => loyalty.Id).IsUnique();
        builder.HasKey(loyalty => loyalty.Id);

        builder.Property(loyalty => loyalty.Id).IsRequired();

        builder.Property(loyalty => loyalty.Username).IsRequired();
    }
}

