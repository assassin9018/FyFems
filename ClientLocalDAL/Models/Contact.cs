namespace ClientLocalDAL.Models;

#nullable disable
public class Contact : EntityBase
{
    public int DialogId { get; set; } = -1;
    public int UserId { get; set; }
    public int ToId { get; set; }
    public virtual User User { get; set; }
    public virtual User To { get; set; }
    /// <summary>
    /// Может быть не заполнен.
    /// </summary>
    public virtual Dialog Dialog { get; set; }
}
