using AutoMapper;

namespace MyFemsApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(nameof(UserDto.BirthDate),
                       x => x.MapFrom(v => v.BirthDate.ToDateTime(TimeOnly.MinValue)));
        CreateMap<RegUserRequest, User>()
            .ForMember(nameof(User.BirthDate),
                       x => x.MapFrom(v => DateOnly.FromDateTime(v.BirthDate)));

        CreateMap<MessageRequest, Message>()
            .ForMember(nameof(Message.Attachments), x => x.MapFrom(v => v.Attachments.ToArray()))
            .ForMember(nameof(Message.Images), x => x.MapFrom(v => v.Images.ToArray()));
        CreateMap<Message, MessageDto>()
            .ForMember(nameof(MessageDto.Attachments), x => x.MapFrom(c => c.Attachments.ToList()))
            .ForMember(nameof(MessageDto.Images), x => x.MapFrom(v => v.Images.ToArray()));

        CreateMap<Dialog, DialogDto>();
        CreateMap<Dialog, DialogLastModifiedOnly>();
        CreateMap<Dialog, DialogUsersOnly>()
            .ForMember(nameof(DialogUsersOnly.UsersId), x => x.MapFrom(v => v.Users.Select(x => x.Id).ToList()));

        CreateMap<Contact, ContactDto>();

        CreateMap<ContactRequest, ContactRequestDto>()
            .ForMember(nameof(ContactRequestDto.Status), x => x.MapFrom(v => (ContactRequestDtoStatus)v.Status));
    }
}
