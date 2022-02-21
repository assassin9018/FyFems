using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientLocalDAL.Models;

#nullable disable
[Index(nameof(DialogId), IsUnique = false, Name = "DialogIndex")]
public class Message : EntityBase
{
    public int From { get; set; }
    [ForeignKey(nameof(From))]
    public virtual User FromUser { get; set; }
    public bool IsDeleted { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [Required]
    [MaxLength(512)]
    public string Content { get; set; }
    [MaxLength(10)]
    public int[] Attachments { get; set; }
    [MaxLength(10)]
    public int[] Images { get; set; }
    public int DialogId { get; set; }
    public virtual Dialog Dialog { get; set; }
}
