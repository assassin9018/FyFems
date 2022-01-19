using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagesController : BaseController
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unit;

    public MessagesController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unit = unitOfWork;
    }

    [HttpPost("{dialogId}")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageRequest)
    {
        const string userProps = nameof(DAL.Models.User.UserDialogs);
        bool insertAllowed = (await _unit.UserRepository
            .GetAsync(x=>x.Id==RequestUserId, includeProperties: userProps))
            .First().UserDialogs
            .Any(x=>x.Id == messageRequest.DialogId);
        if(!insertAllowed)
            throw new ApiException("Insertion not allowed.");

        Message message = _mapper.Map<Message>(messageRequest);
        message.Created = DateTime.UtcNow;
        _unit.MessageRepository.Save(message);
        await _unit.SaveAsync();

        return Ok();
    }
}