namespace Booking.System.ReservationService.DTO.Models;

public class HotelDto
{
    public Guid HotelUid { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public string City { get; set; }
    
    public string Address { get; set; }
    
    public int Stars { get; set; }
    
    public int Price { get; set; }
    
    public HotelDto(Guid hotelUid,
        string name,
        string country,
        string city,
        string address,
        int stars,
        int price)
    {
        HotelUid = hotelUid;
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Stars = stars;
        Price = price;
    }
}