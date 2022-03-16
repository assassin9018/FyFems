using AutoMapper;
using ClientLocalDAL.Models;
using MyFems.Clients.Shared.Models;
using MyFems.Dto;

namespace MyFems.Clients.Shared;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserDto, UserModel>()
            .ForMember(nameof(UserDto.BirthDate),
                       x => x.MapFrom(v => DateOnly.FromDateTime(v.BirthDate)));
        CreateMap<UserDto, User>()
            .ForMember(nameof(User.BirthDate),
                       x => x.MapFrom(v => DateOnly.FromDateTime(v.BirthDate)));
    }
}
