namespace WebMailApi.Dtos;

public class BookingDto
{
    public Guid  EventId { get; set; } 
    public Guid BookingId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int TicketsQuantity { get; set; } = 1;

    public bool TermsAccepted { get; set; }
    public bool NewsletterSubscribed { get; set; } = false;

    public DateTime CreatedAt { get; set; }
}
