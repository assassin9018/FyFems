using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class UserDto : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime Created { get; set; }
    public int Age { get; set; }
}

public class RegUserRequest : AuthRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public string Name { get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }
}

public class ChangePassRequest : AuthRequest
{
    [Required]
    [Range(8, 32)]
    public string NewPassword { get; set; }
}