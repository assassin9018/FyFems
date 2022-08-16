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

        CreateMap<MessageRequest, Message>();
        CreateMap<Message, MessageDto>();

        CreateMap<Dialog, DialogDto>();
        CreateMap<Dialog, DialogLastModifiedOnly>();
        CreateMap<Dialog, DialogUsersOnly>()
            .ForMember(nameof(DialogUsersOnly.UsersId), x => x.MapFrom(v => v.Users.Select(x => x.Id).ToList()));

        CreateMap<Contact, ContactDto>();

        CreateMap<ContactRequest, ContactRequestDto>()
            .ForMember(nameof(ContactRequestDto.Status), x => x.MapFrom(v => (ContactRequestDtoStatus)v.Status));
    }
}
