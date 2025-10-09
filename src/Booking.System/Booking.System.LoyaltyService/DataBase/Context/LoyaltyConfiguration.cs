namespace Booking.System.LoyaltyService.DataBase.Context;

public class LoyaltyConfiguration : IEntityTypeConfiguration<>
{
    public void Configure(EntityTypeBuilder<> builder)
    {
        builder.HasIndex(person => person.Id).IsUnique();
        builder.HasKey(person => person.Id);

        builder.Property(person => person.Id).IsRequired();

        builder.Property(person => person.Name).IsRequired();
    }
}

