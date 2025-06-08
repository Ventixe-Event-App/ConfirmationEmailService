
namespace WebMailApi.Services
{
    public interface IBookingConfirmationService1
    {
        Task SendConfirmationAsync(Guid bookingId);
    }
}