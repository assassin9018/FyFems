namespace DAL.Models;

public class Contact : EntityBase
{
    public int DialogId { get; set; } = -1;
    public int UserId { get; set; }
    public int ToId { get; set; }
    public virtual User User { get; set; }
    public virtual User To { get; set; }
    public virtual Dialog? Dialog { get; set; }
}
