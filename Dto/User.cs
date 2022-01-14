using System;
using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class User : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime Created { get; set; }
    public int Age { get; set; }
}

public class RegUser : AuthRequest
{
    [Required]
    [Range(1, 32)]
    public string Name { get; set; }
    [Required]
    [Range(1, 32)]
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
}