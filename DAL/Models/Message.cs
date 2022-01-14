using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

[Index(nameof(DialogId), IsUnique = false, Name = "DialogIndex")]
public class Message : EntityBase
{
    public int From { get; set; }
    [ForeignKey(nameof(From))]
    public virtual User FromUser { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
    [Required]
    [MaxLength(512)]
    public string Content { get; set; }
    [MaxLength(10)]
    public int[] Attachments { get; set; }
    [MaxLength(10)]
    public int[] Images { get; set; }
    public int DialogId { get; set; }
    public virtual Dialog Dialog { get; set; }

    //public int AttachmentsCount { get; }
    //public int ImagesCount { get; }
}
