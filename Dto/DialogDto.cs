namespace MyFems.Dto;

public class DialogDto : BaseDto
{
    public string Name { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsPrivate { get; set; }
}

public class DialogLastModifiedOnly : BaseDto
{
    public DateTime LastModified { get; set; }
}

public class DialogUsersOnly : BaseDto
{
    public List<int> UsersId { get; set; }
}

public class ConversationRequest
{
    public string Name { get; set; }
    public List<int> UserIds { get; set; }
}
