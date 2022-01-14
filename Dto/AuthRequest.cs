using System;
using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class AuthRequest
{
    [Required]
    [MaxLength(48)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Range(8, 32)]
    public string Password { get; set; }
}
