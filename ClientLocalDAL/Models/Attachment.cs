using System.ComponentModel.DataAnnotations;

namespace ClientLocalDAL.Models;

#nullable disable
public class Attachment : EntityBase
{
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid LocalFileName { get; set; }
}
