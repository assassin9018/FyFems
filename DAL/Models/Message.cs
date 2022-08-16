using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

#nullable disable
[Index(nameof(DialogId), IsUnique = false, Name = "DialogIndex")]
public class Message : EntityBase
{
    public int From { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
    [Required]
    [MaxLength(512)]
    public string Content { get; set; }
    public int DialogId { get; set; }

    [ForeignKey(nameof(From))]
    public virtual User FromUser { get; set; }
    public virtual Dialog Dialog { get; set; }
    public virtual List<Image> Images { get; set; } = new();
    public virtual List<Attachment> Attachments { get; set; } = new();
}
