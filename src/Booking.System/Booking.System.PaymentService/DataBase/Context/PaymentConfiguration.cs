using Booking.System.PaymentService.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.System.PaymentService.DataBase.Context;

public class PaymentConfiguration : IEntityTypeConfiguration<DbPayment>
{
    public void Configure(EntityTypeBuilder<DbPayment> builder)
    {
        builder.HasIndex(loyalty => loyalty.Id).IsUnique();
        builder.HasKey(loyalty => loyalty.Id);

        builder.Property(loyalty => loyalty.Id).IsRequired();
    }
}