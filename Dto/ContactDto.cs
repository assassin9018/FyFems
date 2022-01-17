using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class ContactDto : BaseDto
{
    public int FromId { get; set; }
    public string FromFullName { get; set; }
    public int ToId { get; set; }
    public string ToFullName { get; set; }
}

public class ContactRequestDto : ContactDto
{
    public ContactRequestDtoStatus Status { get; set; }
}

public enum ContactRequestDtoStatus
{
    [Display(Name = "Ожидает подтверждения")]
    Wait = 0,
    [Display(Name = "Подтверждено")]
    Applied = 1,
    [Display(Name = "Отклонено")]
    Declined = 2
}
