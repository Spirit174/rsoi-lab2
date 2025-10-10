using Booking.System.PaymentService.DataBase.Models.Enums;

namespace Booking.System.PaymentService.DataBase.Models;

public class DbPayment
{
    public Guid Id { get; set; }
    
    public Guid PaymentUid { get; set; }
    
    public DbPaymentStatus PaymentStatus { get; set; }
    
    public int Price { get; set; }

    public DbPayment(Guid id,
        Guid paymentUid,
        DbPaymentStatus paymentStatus,
        int price)
    {
        Id = id;
        PaymentUid = paymentUid;
        PaymentStatus = paymentStatus;
        Price = price;
    } 
}