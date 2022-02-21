using AutoMapper;

namespace MyFemsApi.Services.Impl;

internal class ContactsService : BaseService, IContactsService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public ContactsService(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ContactDto>> GetContacts(int requestUserId)
    {
        IEnumerable<Contact>? contacts = await _unitOfWork.ContactsRepository.GetAsync(x => x.UserId == requestUserId);

        return contacts.Select(x => _mapper.Map<Contact, ContactDto>(x)).ToList();
    }

    public async Task<List<ContactRequestDto>> GetRequests(int requestUserId)
    {
        IEnumerable<ContactRequest>? requests = await _unitOfWork.ContactRequestsRepository.GetAsync(x => x.ToId == requestUserId || x.FromId == requestUserId)
            ?? throw new ApiException();

        var result = requests.Select(x => _mapper.Map<ContactRequest, ContactRequestDto>(x)).ToList();

        return result;
    }

    public async Task<ContactRequestDto> SendRequest(int toUserId, int fromUserId)
    {
        if((await _unitOfWork.UserRepository.FindAsync(toUserId)) is null)
            throw new ApiException($"User with id - {toUserId} not found");

        ContactRequest? existedRequest = (await _unitOfWork.ContactRequestsRepository
            .GetAsync(x => x.ToId == toUserId && x.FromId == fromUserId || x.ToId == fromUserId && x.FromId == toUserId))
            .FirstOrDefault();
        if(existedRequest is not null)
            if(existedRequest.FromId == fromUserId)
                return _mapper.Map<ContactRequestDto>(existedRequest);
            else
                return await ApplyRequest(existedRequest.Id, fromUserId);

        ContactRequest cRequest = new()
        {
            FromId = fromUserId,
            ToId = toUserId,
        };
        await _unitOfWork.ContactRequestsRepository.SaveAsync(cRequest);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ContactRequestDto>(cRequest);
    }

    public async Task<ContactRequestDto> ApplyRequest(int requestId, int requestUserId)
    {
        ContactRequest? cRequest = await _unitOfWork.ContactRequestsRepository.FindAsync(requestId);
        if(cRequest is null)
            throw new NotFoundException();

        if(cRequest.ToId != requestUserId)
            throw new ApiException();

        if(cRequest.Status == ContactRequestStatus.Applied)
            throw new ApiException();

        cRequest.Status = ContactRequestStatus.Applied;
        _unitOfWork.ContactRequestsRepository.Save(cRequest);
        Contact contact = new()
        {
            ToId = cRequest.ToId,
            UserId = cRequest.FromId,
        };
        _unitOfWork.ContactsRepository.Save(contact);
        Contact reverceContact = new()
        {
            ToId = cRequest.FromId,
            UserId = cRequest.ToId,
        };
        _unitOfWork.ContactsRepository.Save(reverceContact);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ContactRequestDto>(cRequest);
    }

    public async Task<ContactRequestDto> DeclineRequest(int requestId, int requestUserId)
    {
        ContactRequest? cRequest = await _unitOfWork.ContactRequestsRepository.FindAsync(requestId);
        if(cRequest is null)
            throw new NotFoundException();

        if(cRequest.ToId != requestUserId)
            throw new ApiException();

        var oldStatus = cRequest.Status;
        cRequest.Status = ContactRequestStatus.Applied;
        _unitOfWork.ContactRequestsRepository.Save(cRequest);

        if(oldStatus == ContactRequestStatus.Applied)
        {
            int from = cRequest.FromId, to = cRequest.ToId;
            var contacts = (await _unitOfWork.ContactsRepository
                .GetAsync(x => x.UserId == from && x.ToId == to || x.UserId == from && x.ToId == to, includeProperties: nameof(Contact.Dialog)))
                .ToList();

            Dialog? dialog = contacts.First().Dialog;
            if(dialog is not null)
            {
                dialog.IsDeleted = true;
                _unitOfWork.DialogRepository.Save(dialog);
            }
            _unitOfWork.ContactsRepository.Delete(contacts);
        }
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ContactRequestDto>(cRequest);
    }
}
