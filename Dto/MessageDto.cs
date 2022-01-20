using System;
using System.Collections.Generic;

namespace MyFems.Dto;

public class MessageDto : MessageRequest
{
    public int Id { get; set; }
    public int From { get; set; }
    public DateTime Created { get; set; }
}

public class MessageRequest
{
    public string Content { get; set; }
    public List<int> Attachments { get; set; }
    public List<int> Images { get; set; }
    public int DialogId { get; set; }
}
