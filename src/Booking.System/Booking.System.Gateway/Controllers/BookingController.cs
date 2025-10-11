using System.Security.Cryptography.X509Certificates;
using Booking.System.Gateway.ApiClients;
using Booking.System.Gateway.DTO;
using Booking.System.LoyaltyService.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Booking.System.Gateway.Controllers;

[ApiController]
[Route("/api/v1")]
public class BookingController: ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly ILoyaltyClient _loyaltyClient;
    private readonly IPaymentClient _paymentClient;
    private readonly IReservationClient _reservationClient;

    public BookingController(ILogger<BookingController> logger,
        ILoyaltyClient loyaltyClient,
        IPaymentClient paymentClient,
        IReservationClient reservationClient)
    {
        _logger = logger;
        _loyaltyClient = loyaltyClient;
        _paymentClient = paymentClient;
        _reservationClient = reservationClient;
    }

    /// <summary>
    /// Получить список отелей.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="size">Размер страницы.</param>
    /// <response code="200">Список отелей успешно получен.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/hotels")]
    [SwaggerOperation("Метод для получения списка отелей.", "Метод для получения списка отелей.")]
    [SwaggerResponse(statusCode: 200, description: "Список отелей успешно получен.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<HotelPagesDto>> GetHotels([FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var pages = await _reservationClient.GetHotelsPageAsync(page, size);
            return Ok(pages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request in reservation service");

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Получить информацию о пользователе.
    /// </summary>
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
        
        var loyaltyInfo = await _loyaltyClient.GetLoyaltyAsync(username);
        
        var userInfo = await _userService.GetUserInfoAsync(username);
        return Ok(userInfo);
    }
    
    /// <summary>
    /// Получить информацию по всем бронированиям пользователя.
    /// </summary>
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
    [HttpPost("/reservations")]
    [SwaggerOperation("Метод для бронирования отеля.", "Метод для бронирования отеля.")]
    [SwaggerResponse(statusCode: 201, description: "Бронирование успешно создано.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок или невалидные данные запроса.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateReservationRequest request)
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        try
        {
            var loyaltyInfo = await _loyaltyClient.GetLoyaltyAsync(username);
            
            var reservation = await _reservationService.CreateReservationAsync(username, request);
            
            await _loyaltyClient.UpdateLoyaltyReservationCountAsync(username, true);
            
            return CreatedAtAction(nameof(GetReservation), new { reservationUid = reservation.ReservationUid }, reservation);
        }
        catch ( ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request in loyalty service");

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Отменить бронирование.
    /// </summary>
    /// <param name="reservationUid">UID бронирования для отмены.</param>
    /// <response code="204">Бронирование успешно отменено.</response>
    /// <response code="400">Отсутствует заголовок.</response>
    /// <response code="404">Бронирование не найдено.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpDelete("/reservations/{reservationUid}")]
    [SwaggerOperation("Метод для отмены бронирования отеля.", "Метод для отмены бронирования отеля.")]
    [SwaggerResponse(statusCode: 204, description: "Бронирование успешно отменено.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Бронирование не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CancelReservation(Guid reservationUid)
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");

        var success = await _reservationService.CancelReservationAsync(reservationUid, username);
        if (!success)
            return NotFound();
        
        await _loyaltyClient.UpdateLoyaltyReservationCountAsync(username, false);

        return NoContent();
    }
    
    /// <summary>
    /// Получить информацию о статусе в программе лояльности.
    /// </summary>
    /// <response code="200">Информация о статусе лояльности успешно получена.</response>
    /// <response code="400">Отсутствует заголовок.</response>
    /// <response code="404">Информация о программе лояльности не найдена.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("/loyalty")]
    [SwaggerOperation("Метод для получения статуса лояльности.", "Метод для получения статуса лояльности.")]
    [SwaggerResponse(statusCode: 200, description: "Статус лояльности успешно получен.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Отсутствует заголовок.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<ActionResult<LoyaltyInfoDto>> GetLoyaltyInfo()
    {
        var username = Request.Headers["X-User-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(username))
            return BadRequest("X-User-Name header is required");
        
        try
        {
            var loyaltyInfo = await _loyaltyClient.GetLoyaltyAsync(username);
            return Ok(loyaltyInfo);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request in loyalty service");

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    [HttpGet("manage/health")]
    public IActionResult Health()
    {
        return Ok();
    }
    
}