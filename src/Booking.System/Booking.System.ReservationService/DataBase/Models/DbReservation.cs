using System.ComponentModel.DataAnnotations.Schema;
using Booking.System.ReservationService.DataBase.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Booking.System.ReservationService.DataBase.Models;

public class DbReservation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public Guid ReservationUid { get; set; }
    
    public string Username { get; set; }
    
    public Guid PaymentUid { get; set; }
    
    public Guid HotelUid { get; set; }
    
    public DbPaymentStatus Status { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    [ForeignKey("HotelId")]
    public virtual DbHotel Hotel { get; set; } = null!;

    public DbReservation(int id,
        Guid reservationUid,
        string username,
        Guid paymentUid,
        Guid hotelUid,
        DbPaymentStatus status,
        DateTime startDate,
        DateTime endDate)
    {
        Id = id;
        ReservationUid = reservationUid;
        Username = username;
        Status = status;
        PaymentUid = paymentUid;
        HotelUid = hotelUid;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
    }
}