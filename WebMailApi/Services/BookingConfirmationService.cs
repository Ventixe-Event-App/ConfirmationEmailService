using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime;
using WebMailApi.Dtos;

namespace WebMailApi.Services;

public interface IBookingConfirmationService
{
    Task SendConfirmationAsync(Guid bookingId);
}

public class BookingConfirmationService : IBookingConfirmationService 
{
    private readonly EmailClient _emailClient;
    private readonly EmailSettings _settings;
    private readonly HttpClient _bookingHttp; // Used to call the external BookingService and EventService APIs
    private readonly HttpClient _eventHttp;

    public BookingConfirmationService(IOptions<EmailSettings> settings, IHttpClientFactory httpFactory)
    {
        _settings = settings.Value;
        _emailClient = new EmailClient(_settings.ConnectionString);
        _bookingHttp = httpFactory.CreateClient("BookingService");
        _eventHttp = httpFactory.CreateClient("EventService");
    }

    public async Task SendConfirmationAsync(Guid bookingId)
    {

        var booking = await _bookingHttp
            .GetFromJsonAsync<BookingDto>($"/api/bookings/{bookingId}");

        // Lägg till denna kontroll:
        if (booking == null)
            throw new InvalidOperationException($"Booking {bookingId} not found");

        // Hämta event:
        var evt = await _eventHttp
            .GetFromJsonAsync<EventDto>($"/api/events/{booking.EventId}");


        if (evt == null)
            throw new InvalidOperationException($"Event {booking.EventId} not found");


        // 3. Build HTML body
        var html = $@"
        <h1>Booking Confirmed!</h1>
        <p>Hi {booking.FirstName},</p>
        <p>Thanks for booking <strong>{evt.EventTitle}</strong> on 
           {evt.EventDate:MMMM d, yyyy} at {evt.Location}.</p>
        <p>Tickets: {booking.TicketsQuantity}</p>
        <p>See you there!</p>";

        // 4. Send email - Fixa EmailClient.SendAsync anropet
        var emailMessage = new EmailMessage(
            senderAddress: _settings.SenderAddress,
            recipientAddress: booking.Email,
            content: new EmailContent($"Your booking for {evt.EventTitle}")
            {
                Html = html
            });

        await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
    }
}
