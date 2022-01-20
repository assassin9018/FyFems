using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

#nullable disable
public class Attachment : EntityBase
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; }
    [Required]
    [MaxLength(2 * 1024 * 1024)]//2MB
    public byte[] Data { get; set; }
}
