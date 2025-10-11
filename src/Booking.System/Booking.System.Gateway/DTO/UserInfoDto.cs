using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Booking.System.Gateway.DTO;

public class UserInfoDto
{
    /// <summary>
    /// Список броней.
    /// </summary>
    [Required]
    [DataMember(Name = "hotel")]
    [JsonPropertyName("hotel")]
    public List<ReservationDtoWithHotelAndPayment> Hotels { get; set; }
    
    /// <summary>
    /// Информация о скидке.
    /// </summary>
    [Required]
    [DataMember(Name = "loyalty")]
    [JsonPropertyName("loyalty")]
    public LoyaltyInfoDto LoyaltyInfo { get; set; }
    
    public UserInfoDto(List<ReservationDtoWithHotelAndPayment> hotels,
        LoyaltyInfoDto loyalty)
    {
        Hotels = hotels;
        LoyaltyInfo = loyalty;
    }
}