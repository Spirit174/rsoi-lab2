using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Booking.System.Gateway.Controllers;

[ApiController]
[Route("/api/v1")]
public class BookingController: ControllerBase
{
    private readonly ILogger<BookingController> _logger;

    public BookingController(ILogger<BookingController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Получить список отелей.
    /// </summary>
    /// <remarks>Метод для получения пагинированного списка отелей.</remarks>
    /// <param name="page">Номер страницы.</param>
    /// <param name="size">Размер страницы.</param>
    /// <response code="200">Список отелей успешно получен.</response>
    /// <response code="400">Неверные параметры.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/hotels")]
    [SwaggerOperation("Метод для получения списка отелей.", "Метод для получения списка отелей.")]
    [SwaggerResponse(statusCode: 200, description: "Список отелей успешно получен.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Неверные параметры.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<HotelDto>> GetHotels([FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        var hotels = await _hotelService.GetHotelsAsync(page, size);
        return Ok(hotels);
    }
    
    /// <summary>
    /// Получить информацию о пользователе.
    /// </summary>
    /// <remarks>Метод возвращает информацию о бронированиях и статусе в системе лояльности.</remarks>
    /// <response code="200">Информация о пользователе успешно получена.</response>
    /// <response code="400">Отсутствует заголовок.</response>
    /// <response code="404">Пользователь не найден.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/me")]
    [SwaggerOperation("Метод для получения информации о пользователе.", "Метод для получения информации о пользователе.")]
    [SwaggerResponse(statusCode: 200, description: "Информация о пользователе успешно получена.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        var userInfo = await _userService.GetUserInfoAsync(username);
        return Ok(userInfo);
    }
    
    /// <summary>
    /// Получить информацию по всем бронированиям пользователя.
    /// </summary>
    /// <remarks>Метод возвращает список всех бронирований текущего пользователя.</remarks>
    /// <response code="200">Список бронирований успешно получен.</response>
    /// <response code="400">Отсутствует заголовок.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/reservations")]
    [SwaggerOperation("Метод для получения информации о всех бронированиях пользователя.", "Метод для получения информации о всех бронированиях пользователя.")]
    [SwaggerResponse(statusCode: 200, description: "Список бронирований успешно получен.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<List<ReservationDto>>> GetUserReservations()
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        var reservations = await _reservationService.GetUserReservationsAsync(username);
        return Ok(reservations);
    }

    /// <summary>
    /// Получить информацию по конкретному бронированию.
    /// </summary>
    /// <param name="reservationUid">Id бронирования.</param>
    /// <response code="200">Информация о бронировании успешно получена.</response>
    /// <response code="400">Отсутствует заголовок.</response>
    /// <response code="404">Бронирование не найдено.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/reservations/{reservationUid}")]
    [SwaggerOperation("Метод для получения информации о конкретном бронирование пользователя.", "Метод для получения информации о конкретном бронирование пользователя.")]
    [SwaggerResponse(statusCode: 200, description: "Информация о бронировании успешно получена.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Бронирование не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<ReservationDto>> GetReservation([FromRoute] Guid reservationUid)
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        var reservation = await _reservationService.GetReservationAsync(reservationUid, username);
        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }
    
    /// <summary>
    /// Забронировать отель.
    /// </summary>
    /// <param name="request">Данные для бронирования отеля.</param>
    /// <response code="201">Бронирование успешно создано.</response>
    /// <response code="400">Отсутствует заголовок или невалидные данные запроса.</response>
    /// <response code="404">Отель не найден.</response>
    /// <response code="409">Конфликт при создании бронирования.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPost("/hotels")]
    [SwaggerOperation("Метод для бронирования отеля.", "Метод для бронирования отеля.")]
    [SwaggerResponse(statusCode: 201, description: "Бронирование успешно создано.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок или невалидные данные запроса.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Отель не найден.")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponse), description: "Конфликт при создании бронирования")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateReservationRequest request)
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        try
        {
            var reservation = await _reservationService.CreateReservationAsync(username, request);
            return CreatedAtAction(nameof(GetReservation), new { reservationUid = reservation.ReservationUid }, reservation);
        }
        catch ( ex)
        {
            return BadRequest(ex.Message);
        }
        catch ( ex)
        {
            return Conflict(ex.Message);
        }
    }
    
}