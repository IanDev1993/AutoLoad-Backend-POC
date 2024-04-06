public class OrderDriver{   
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Name { get; set; } = null!;    
    public string Document { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}