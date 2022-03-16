using AutoMapper;
using ClientLocalDAL.Models;
using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyFems.Dto;
using MyFems.Models;
using RestApiClient;
using System.Collections.ObjectModel;

namespace MyFems.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMyFemsFullClient _client;
    private readonly IMapper _mapper;

    public ObservableCollection<UserModel> Users { get; set; } = new();
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();
    public UserModel UserModel { get; } = new();

    public MainViewModel(IMyFemsFullClient client, UnitOfWork unitOfWork, IMapper mapper)
    {
        _client = client;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        var contactEntities = unitOfWork.ContactsRepository.Get();
        IEnumerable<ContactModel> mappedContacts = contactEntities.Select(entity => _mapper.Map<Contact, ContactModel>(entity));
        Contacts = new(mappedContacts);

        var userEntities = unitOfWork.UserRepository.Get();
        IEnumerable<UserModel> mappedUsers = userEntities.Select(entity => _mapper.Map<User, UserModel>(entity));
        Users = new(mappedUsers);
    }

    #region Comands

    private IAsyncRelayCommand? _updateContactsCommand;

    public IAsyncRelayCommand UpdateContactsCommand
        => _updateContactsCommand ??= new AsyncRelayCommand(async (CancalationToken) =>
        {
            List<ContactDto> contactsDto = await _client.GetContacts();
            var existed = Contacts.Select(x => x.Id).ToHashSet();
            if(existed.Count == contactsDto.Count && contactsDto.All(x => existed.Contains(x.Id)))
                return;

            IEnumerable<Contact> notExistedContacts = contactsDto
               .Where(x => !existed.Contains(x.Id)).Select(dto => _mapper.Map<ContactDto, Contact>(dto));

            await _unitOfWork.ContactsRepository.SaveAllAsync(notExistedContacts);
            await _unitOfWork.SaveAsync();

            Contacts.Clear();
            var contactEntities = _unitOfWork.ContactsRepository.Get();
            IEnumerable<ContactModel> mappedContacts = contactEntities.Select(entity => _mapper.Map<Contact, ContactModel>(entity));
            Contacts = new(mappedContacts);
            foreach(var contact in mappedContacts)
                Contacts.Add(contact);
        });

    #endregion
}
