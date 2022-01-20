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
    public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
    {
        const string userProps = nameof(DAL.Models.User.UserDialogs);
        int fromUserId = RequestUserId;
        Dialog? dialog = (await _unit.UserRepository
            .GetAsync(x => x.Id == fromUserId, includeProperties: userProps))
            .First().UserDialogs
            .FirstOrDefault(x => x.Id == messageRequest.DialogId);
        if(dialog is null)
            throw new NotFoundException("Dialog not found.");

        Message message = _mapper.Map<Message>(messageRequest);
        message.From = fromUserId;
        dialog.LastModified = DateTime.UtcNow;
        _unit.MessageRepository.Save(message);
        _unit.DialogRepository.Save(dialog);
        await _unit.SaveAsync();

        return Ok();
    }
}