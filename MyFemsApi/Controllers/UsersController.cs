using AutoMapper;
using DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DtoUser = MyFems.Dto.User;
using ModelUser = DAL.Models.User;

namespace MyFemsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public UsersController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Post([Required, FromBody] MyFems.Dto.RegUser user)
    {
        var dbUser = _mapper.Map<MyFems.Dto.RegUser, ModelUser>(user);
        await _unitOfWork.UserRepository.SaveAsync(dbUser);
        return Ok(_mapper.Map<ModelUser, DtoUser>(dbUser));
    }

    [HttpGet("{searchText}")]
    public async Task<IActionResult> Search(string searchText)
    {
        var users = (await _unitOfWork.UserRepository.GetAsync(u => u.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))).ToList();

        List<DtoUser> dtoUsers = new(users.Count);
        foreach (var user in users)
            dtoUsers.Add(_mapper.Map<ModelUser, DtoUser>(user));
        
        return Ok(dtoUsers);
    }
}
