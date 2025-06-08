namespace WebMailApi.Dtos;

public class EventDto
{
    public Guid EventId { get; set; } 
    public string EventTitle { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public decimal? Price { get; set; }
    public string Location { get; set; } = null!;
}
