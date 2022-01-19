using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ContactsController : BaseController
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public ContactsController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetContacts()
    {
        IEnumerable<Contact>? contacts = await _unitOfWork.ContactsRepository.GetAsync(x => x.UserId == RequestUserId);

        var result = contacts.Select(x => _mapper.Map<Contact, ContactDto>(x)).ToList();

        return Ok(result);
    }

    [HttpGet("requests")]
    public async Task<ActionResult<List<ContactRequestDto>>> GetRequests()
    {
        int requestUserId = RequestUserId;
        IEnumerable<ContactRequest>? requests = await _unitOfWork.ContactRequestsRepository.GetAsync(x => x.ToId == requestUserId || x.FromId == requestUserId)
            ?? throw new ApiException();

        var result = requests.Select(x=> _mapper.Map<ContactRequest, ContactRequestDto>(x)).ToList();

        return Ok(result);
    }

    [HttpPost("requests/send/{toUserId}")]
    public async Task<ActionResult<ContactRequestDto>> SendRequest(int toUserId)
    {
        if((await _unitOfWork.UserRepository.FindAsync(toUserId)) is null)
            throw new ApiException($"User with id - {toUserId} not found");

        int fromUserId = RequestUserId;
        ContactRequest? existedRequest = (await _unitOfWork.ContactRequestsRepository
            .GetAsync(x=>x.ToId == toUserId && x.FromId == fromUserId || x.ToId == fromUserId && x.FromId == toUserId ))
            .FirstOrDefault();
        if(existedRequest is not null)
            if(existedRequest.FromId == fromUserId)
                return Ok(existedRequest);
            else
                return await ApplyRequest(existedRequest.Id);

        ContactRequest cRequest = new()
        {
            FromId = fromUserId,
            ToId = toUserId,
        };
        await _unitOfWork.ContactRequestsRepository.SaveAsync(cRequest);
        await _unitOfWork.SaveAsync();

        return Ok(_mapper.Map<ContactRequestDto>(cRequest));
    }

    [HttpPatch("requests/apply/{requestId}")]
    public async Task<ActionResult<ContactRequestDto>> ApplyRequest(int requestId)
    {
        ContactRequest? cRequest = await _unitOfWork.ContactRequestsRepository.FindAsync(requestId);
        if(cRequest is null)
            throw new NotFoundException();

        if(cRequest.ToId != RequestUserId)
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

        return Ok(_mapper.Map<ContactRequestDto>(cRequest));
    }

    [HttpPatch("requests/decline/{requestId}")]
    public async Task<ActionResult<ContactRequestDto>> DeclineRequest(int requestId)
    {
        ContactRequest? cRequest = await _unitOfWork.ContactRequestsRepository.FindAsync(requestId);
        if(cRequest is null)
            throw new NotFoundException();

        if(cRequest.ToId != RequestUserId)
            throw new ApiException();

        var oldStatus = cRequest.Status;
        cRequest.Status = ContactRequestStatus.Applied;
        _unitOfWork.ContactRequestsRepository.Save(cRequest);

        if(oldStatus == ContactRequestStatus.Applied)
        {
            int from = cRequest.FromId, to = cRequest.ToId;
            var contacts = (await _unitOfWork.ContactsRepository
                .GetAsync(x=>x.UserId == from && x.ToId == to || x.UserId == from && x.ToId == to, includeProperties:nameof(Contact.Dialog)))
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

        return Ok(_mapper.Map<ContactRequestDto>(cRequest));
    }
}
