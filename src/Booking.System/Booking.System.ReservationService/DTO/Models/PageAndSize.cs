using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Booking.System.ReservationService.DTO.Models;

public class PageAndSize
{
    /// <summary>
    /// Страница.
    /// </summary>
    [Required]
    [DataMember(Name = "page")]
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    /// <summary>
    /// Размер страницы.
    /// </summary>
    [Required]
    [DataMember(Name = "size")]
    [JsonPropertyName("size")]
    public int Size { get; set; }

    public PageAndSize(int page,
        int size)
    {
        Page = page;
        Size = size;
    }
}