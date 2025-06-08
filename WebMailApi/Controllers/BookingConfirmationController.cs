// BookingConfirmationController.cs
using Microsoft.AspNetCore.Mvc;
using WebMailApi.Services;

[ApiController]
[Route("api/[controller]")]
public class BookingConfirmationController : ControllerBase
{
    private readonly IBookingConfirmationService _confirmService;

    public BookingConfirmationController(IBookingConfirmationService confirmService)
    {
        _confirmService = confirmService;
    }

    [HttpPost("{bookingId}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(Guid bookingId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // you might want to check existence first, or let service throw if not found
        await _confirmService.SendConfirmationAsync(bookingId);
        return Accepted();
    }
}
