using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Booking.System.Gateway.DTO;

public class CreateReservationResponse
{
    /// <summary>
    /// Идентификатор брони.
    /// </summary>
    [Required]
    [DataMember(Name = "reservationUid")]
    [JsonPropertyName("reservationUid")]
    public Guid ReservationUid { get; set; }
    
    /// <summary>
    /// Идентификатор отеля.
    /// </summary>
    [Required]
    [DataMember(Name = "hotelUid")]
    [JsonPropertyName("hotelUid")]
    public Guid HotelUid { get; set; }
    
    /// <summary>
    /// Начало бррони.
    /// </summary>
    [Required]
    [DataMember(Name = "startDate")]
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Конец бррони.
    /// </summary>
    [Required]
    [DataMember(Name = "endDate")]
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// Скидка.
    /// </summary>
    [Required]
    [DataMember(Name = "discount")]
    [JsonPropertyName("discount")]
    public int Discount { get; set; }
    
    // <summary>
    /// Статус.
    /// </summary>
    [Required]
    [DataMember(Name = "status")]
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    // <summary>
    /// Платеж.
    /// </summary>
    [Required]
    [DataMember(Name = "payment")]
    [JsonPropertyName("payment")]
    public PaymentInfoDto PaymentInfo { get; set; }

    public CreateReservationResponse(Guid reservationUid,
        Guid hotelUid,
        DateTime startDate,
        DateTime endDate,
        int discount,
        string status,
        PaymentInfoDto paymentInfo)
    {
        ReservationUid = reservationUid;
        HotelUid = hotelUid;
        StartDate = startDate;
        EndDate = endDate;
        Discount = discount;
        Status = status;
        PaymentInfo = paymentInfo;
    }
}