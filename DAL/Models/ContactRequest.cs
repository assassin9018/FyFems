namespace DAL.Models;

public class ContactRequest : EntityBase
{
    public int FromId { get; set; }
    public virtual User FromUser { get; set; }
    public int ToId { get; set; }
    public virtual User ToUser { get; set; }
    public ContactRequestStatus Status { get; set; }
}

public enum ContactRequestStatus
{
    Wait = 0,
    Applied = 1,
    Declined = 2
}
