namespace DAL.Models;

public class Contact : EntityBase
{
    public int FromId { get; set; }
    public int ToId { get; set; }
    public virtual User From { get; set; }
    public virtual User To { get; set; }
}
