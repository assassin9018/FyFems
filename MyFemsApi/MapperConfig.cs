using AutoMapper;
using DAL.Models;

namespace MyFemsApi;

public class MapperConfig
{
    public static void Init(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, MyFems.Dto.User>();
        config.CreateMap<MyFems.Dto.RegUser, User>();
    }
}
