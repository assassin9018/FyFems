using AutoMapper;

namespace MyFemsApi.Services.Impl;

internal class DialogsService : BaseService, IDialogsService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unit;

    public DialogsService(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unit = unitOfWork;
    }

    public async Task<List<DialogDto>> GetDialogs(int requestUserId)
    {
        List<Dialog> dialogs = (await _unit.UserRepository
            .GetAsync(x => x.Id == requestUserId, includeProperties: nameof(DAL.Models.User.UserDialogs)))
            .First().UserDialogs;

        return dialogs.Select(x => _mapper.Map<DialogDto>(x)).ToList();
    }

    public async Task<DialogDto> Post(int contactId, int curUserId)
    {
        Contact contact = (await _unit.ContactsRepository
            .FindAsync(contactId)) ?? throw new NotFoundException();

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
        reverceContact.Dialog = dialog;
        _unit.DialogRepository.Save(dialog);
        _unit.ContactsRepository.Save(contact);
        _unit.ContactsRepository.Save(reverceContact);
        await _unit.SaveAsync();

        return _mapper.Map<DialogDto>(dialog);
    }

    public async Task<DialogDto> PostConversation(ConversationRequest request, int requestUserId)
    {
        const string contactsProps = nameof(DAL.Models.User.Contacts);
        User creator = (await _unit.UserRepository.GetAsync(x => x.Id == requestUserId, includeProperties: contactsProps)).First();

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

        return _mapper.Map<DialogDto>(dialog);
    }

    public async Task<DialogUsersOnly> GetDialogUsers(int dialogId, int requestUserId)
    {
        Dialog dialog = (await _unit.DialogRepository
            .GetAsync(x => x.Id == dialogId, includeProperties: nameof(Dialog.Users)))
            .FirstOrDefault() ?? throw new NotFoundException();

        if(!dialog.Users.Any(x => x.Id == requestUserId))
            throw new NotFoundException();

        return _mapper.Map<DialogUsersOnly>(dialog);
    }

    public async Task<List<DialogLastModifiedOnly>> GetLastModificationDates(int requestUserId)
    {
        var userWithDialogs = (await _unit.UserRepository
            .GetAsync(x => x.Id == requestUserId, includeProperties: nameof(DAL.Models.User.UserDialogs)))
            .FirstOrDefault() ?? throw new NotFoundException();

        List<DialogLastModifiedOnly> result = userWithDialogs.UserDialogs
            .Select(x => _mapper.Map<DialogLastModifiedOnly>(x))
            .ToList();

        return result;
    }
}
