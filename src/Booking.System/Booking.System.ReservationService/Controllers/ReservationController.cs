using Booking.System.ReservationService.Core.Interfaces;
using Booking.System.ReservationService.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking.System.ReservationService.Controllers;

public class ReservationController: ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;
    
    public ReservationController(IReservationService reservationService,
        ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }
    
    /// <summary>
    /// Получить информацию о статусе в программе лояльности.
    /// </summary>
    [HttpPost("/hotels/{reservationId}")]
    public async Task<ActionResult> GetHotelsPages([FromRoute] Guid reservationId)
    {
        try
        {
            if (!await _reservationService.CancelReservation(reservationId))
                return BadRequest(new ErrorResponse("Нeт такой брони."));

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request in reservation service");

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Получить информацию о статусе в программе лояльности.
    /// </summary>
    [HttpGet("/hotels/{reservationId}")]
    public async Task<ActionResult> ([FromRoute] Guid reservationId)
    {
        try
        {
            _reservationService.GetReservationByReservationIdAsync()
            if (!await _reservationService.CancelReservation(reservationId))
                return BadRequest(new ErrorResponse("Нeт такой брони."));

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request in reservation service");

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}