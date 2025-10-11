using Booking.System.ReservationService.Core.Models;
using Booking.System.ReservationService.DataBase.Converters.Enums;
using Booking.System.ReservationService.DataBase.Models;

namespace Booking.System.ReservationService.DataBase.Converters;


public class ReservationConverter
{
    public static Reservation Convert(DbReservation reservation)
    {
        return new Reservation(reservation.Id,
            reservation.ReservationUid,
            reservation.Username,
            reservation.PaymentUid,
            reservation.HotelUid,
            PaymentStatusConverter.Convert(reservation.Status),
            reservation.StartDate,
            reservation.EndDate);
    }
    
    public static DbReservation Convert(Reservation reservation)
    {
        return new DbReservation(reservation.Id,
            reservation.ReservationUid,
            reservation.Username,
            reservation.PaymentUid,
            reservation.HotelUid,
            PaymentStatusConverter.Convert(reservation.Status),
            reservation.StartDate,
            reservation.EndDate);
    }
}