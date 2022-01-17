using AutoMapper;

namespace MyFemsApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(nameof(UserDto.BirthDate),
                       x => x.MapFrom(u => u.BirthDate.ToDateTime(TimeOnly.MinValue)));
        CreateMap<RegUserDto, User>()
            .ForMember(nameof(User.BirthDate),
                       x => x.MapFrom(u => DateOnly.FromDateTime(u.BirthDate)));
    }

    public static void Init(IMapperConfigurationExpression config)
    {
        //config.AddProfile<MapperProfile>();


    }
}
