using System.ComponentModel.DataAnnotations;

namespace BackEndMimimal.Models.DTO;

public class OrderDTO
{
    [Required]
    [MinLength(3, ErrorMessage = "Enter at least 3 characters!")]
    [MaxLength(50, ErrorMessage = "Character limit has been exceeded!")]
    public string Code { get; set; } = string.Empty;
    public bool Status { get; set; } = false;
}