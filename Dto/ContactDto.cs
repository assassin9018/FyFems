using NetEscapades.EnumGenerators;
using System.ComponentModel.DataAnnotations;

namespace MyFems.Dto;

public class ContactDto : BaseDto
{
    public int DialogId { get; set; }
    public int UserId { get; set; }
    public int ToId { get; set; }
}

public class ContactRequestDto : BaseDto
{
    public int FromId { get; set; }
    public int ToId { get; set; }
    public ContactRequestDtoStatus Status { get; set; }
}

[EnumExtensions]
public enum ContactRequestDtoStatus
{
    [Display(Name = "Ожидает подтверждения")]
    Wait = 0,
    [Display(Name = "Подтверждено")]
    Applied = 1,
    [Display(Name = "Отклонено")]
    Declined = 2
}
