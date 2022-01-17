﻿using AutoMapper;
using DAL.Models;

namespace MyFemsApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, MyFems.Dto.User>()
            .ForMember(nameof(MyFems.Dto.User.BirthDate),
                       x => x.MapFrom(u => u.BirthDate.ToDateTime(TimeOnly.MinValue)));
        CreateMap<MyFems.Dto.RegUser, User>()
            .ForMember(nameof(User.BirthDate),
                       x => x.MapFrom(u => DateOnly.FromDateTime(u.BirthDate)));
    }

    public static void Init(IMapperConfigurationExpression config)
    {
        //config.AddProfile<MapperProfile>();


    }
}
