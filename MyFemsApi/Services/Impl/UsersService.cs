using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyFemsApi.Services.Impl;

internal class UsersService : BaseService, IUsersService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public UsersService(IMapper mapper, UnitOfWork unitOfWork, IServiceScope scope)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Возвращает информацию о залогиненом пользователе.
    /// </summary>
    /// <returns>Информация о залогиненом пользователе.</returns>
    public async Task<UserDto> WhoAmI(int requestUserId)
    {
        User currentUser = await GetUser(_unitOfWork, requestUserId);
        return _mapper.Map<User, UserDto>(currentUser);
    }

    public async Task<UserDto> GetUser(int userId)
    {
        User? user = await GetUser(_unitOfWork, userId) ?? throw new ApiException();
        return _mapper.Map<User, UserDto>(user);
    }

    public async Task<List<UserDto>> Search(string searchText)
    {
        var users = (await _unitOfWork.UserRepository.GetAsync(u => u.FullName.ToLower().Contains(searchText.ToLower()))).ToList();

        List<UserDto> dtoUsers = new(users.Count);
        foreach(var user in users)
            dtoUsers.Add(_mapper.Map<User, UserDto>(user));

        return dtoUsers;
    }
}