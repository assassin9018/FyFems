using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DialogsController : BaseController
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unit;

    public DialogsController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unit = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<List<DialogDto>>> GetDialogs()
    {
        List<Dialog> dialogs = (await _unit.UserRepository
            .GetAsync(x=>x.Id == RequestUserId, includeProperties:nameof(DAL.Models.User.UserDialogs)))
            .First().UserDialogs;

        return Ok(dialogs.Select(x=>_mapper.Map<DialogDto>(x)).ToList());
    }

    [HttpPost("{contactId}")]
    public async Task<ActionResult<DialogDto>> Post(int contactId)
    {
        Contact contact = (await _unit.ContactsRepository
            .FindAsync(contactId)) ?? throw new NotFoundException();

        int curUserId = RequestUserId;
        if(contact.UserId != curUserId && contact.ToId != curUserId)
            throw new ApiException();
        if(contact.DialogId > 0)
            throw new ApiException();

        const string properties = $"{nameof(Contact.To)},{nameof(Contact.User)}";
        Contact reverceContact = (await _unit.ContactsRepository
            .GetAsync(x => x.UserId == contact.ToId && x.ToId == contact.UserId, includeProperties: properties))
            .First();

        Dialog dialog = new()
        {
            IsPrivate = true,
            LastModified = DateTime.UtcNow,
            Name = string.Empty,
            Users = new List<User>() { reverceContact.To, reverceContact.User }
        };
        contact.Dialog = dialog;
        reverceContact.Dialog= dialog;
        _unit.DialogRepository.Save(dialog);
        _unit.ContactsRepository.Save(contact);
        _unit.ContactsRepository.Save(reverceContact);
        await _unit.SaveAsync();

        return Ok(_mapper.Map<DialogDto>(dialog));
    }

    [HttpPost("conversation")]
    public async Task<ActionResult<DialogDto>> PostConversation(ConversationRequest request)
    {
        const string contactsProps = nameof(DAL.Models.User.Contacts);
        User creator = (await _unit.UserRepository.GetAsync(x=>x.Id == RequestUserId,includeProperties:contactsProps)).First();

        if(!creator.Contacts.Select(x => x.ToId).ToHashSet().SetEquals(request.UserIds))
            throw new ApiException("Creator may add only his own contacts.");

        List<User> dudes = new(request.UserIds.Count + 1) { creator };

        foreach(var userId in request.UserIds)
            dudes.Add((await _unit.UserRepository.FindAsync(userId))!);

        Dialog dialog = new()
        {
            IsPrivate = false,
            LastModified = DateTime.UtcNow,
            Name = request.Name,
            Users = dudes
        };

        return Ok(_mapper.Map<DialogDto>(dialog));
    }

    [HttpGet("users/{dialogId}")]
    public async Task<ActionResult<DialogUsersOnly>> GetDialogUsers(int dialogId)
    {
        Dialog dialog = (await _unit.DialogRepository
            .GetAsync(x => x.Id == dialogId, includeProperties: nameof(Dialog.Users)))
            .FirstOrDefault() ?? throw new NotFoundException();

        if(!dialog.Users.Any(x => x.Id == RequestUserId))
            throw new NotFoundException();

        return Ok(_mapper.Map<DialogUsersOnly>(dialog));
    }

    [HttpGet("modificationDates")]
    public async Task<ActionResult<List<DialogLastModifiedOnly>>> GetLastModificationDates()
    {
        var userWithDialogs = (await _unit.UserRepository
            .GetAsync(x => x.Id == RequestUserId, includeProperties: nameof(DAL.Models.User.UserDialogs)))
            .FirstOrDefault() ?? throw new NotFoundException();

        List<DialogLastModifiedOnly> result = userWithDialogs.UserDialogs
            .Select(x=>_mapper.Map<DialogLastModifiedOnly>(x))
            .ToList();

        return Ok(result);
    }
}
