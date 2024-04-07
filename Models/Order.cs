namespace BackEndMimimal.Models;

public class Order
{
    public Order()
    {
        Id = Guid.NewGuid();      
        CreatedAt = DateTime.Now;
    }
    public Guid Id { get; set; } 
    public string? Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Status { get; set; }
}