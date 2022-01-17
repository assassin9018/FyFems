using System;
using System.Collections.Generic;

namespace MyFems.Dto;

public class DialogDto : BaseDto
{
    public string Name { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsPrivate { get; set; }
    public List<int> UsersId { get; set; }
}
