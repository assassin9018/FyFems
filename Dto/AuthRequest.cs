using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class AuthRequest
{
    [Required]
    [MaxLength(48)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(32)]
    public string Password { get; set; }
}
