public class Order{
    public Guid Id { get; set; }
    public OrderDriver? Driver { get; set; }
    public string Code { get; set; } = null!;
    public DateTime CreatedAt { get; set; } 
    public bool Status { get; set; } 
}