namespace BackEndMimimal.Models;

public class Driver
{
    public Driver()
    {
        Id = Guid.NewGuid();      
        CreatedAt = DateTime.Now;
        Name = string.Empty;
        Document = string.Empty; 
    }
    public Guid Id { get; set; } 
    public string Name { get; set; }    
    public string Document { get; set; }
    public DateTime CreatedAt { get; set; }
    
}