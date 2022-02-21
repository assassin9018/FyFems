using AutoMapper;

namespace MyFemsApi.Services.Impl;

internal class MessagesService : BaseService, IMessagesService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unit;

    public MessagesService(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unit = unitOfWork;
    }

    public async Task SendMessage(MessageRequest messageRequest, int fromUserId)
    {
        const string userProps = nameof(DAL.Models.User.UserDialogs);
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

        return;
    }
}
