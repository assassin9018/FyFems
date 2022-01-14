using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class User : EntityBase
{
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }
    [Required]
    [MaxLength(32)]
    public string Surname { get; set; }
    [Required]
    [MaxLength(48)]
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public DateTime BirthDate { get; set; }
    public string FullName { get; }
    public int Age { get; }
    [Required]
    public string PasswordHash { get; set; }
    public virtual List<Dialog> UserDialogs { get; set; } = new List<Dialog>();
}
