using AutoMapper;
using ClientLocalDAL.Models;
using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyFems.Clients.Shared.Models;
using MyFems.Dto;
using RestApiClient;
using System.Collections.ObjectModel;

namespace MyFems.Clients.Shared.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMyFemsFullClient _client;
    private readonly IMapper _mapper;

    [ObservableProperty]
    private DialogModel? _selectedDialog;
    [ObservableProperty]
    public UserModel _applicationUser = new();

    public ObservableCollection<UserModel> Users { get; set; } = new();
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();
    public ObservableCollection<DialogModel> Dialogs { get; set; } = new();

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

        //todo remove this
        Dialogs = new()
        {
            new()
            {
                Id = 0,
                IsPrivate = false,
                LastModified = DateTime.Now,
                Name = "Беседа",
                Users = new() { new() { }, new() { } },
                Messages = new() { new(null) { Id = 0, Recived = DateTime.Now, SelfMessage = true, Text = "first" }, new(null) { Id = 1, SelfMessage = false, Text = "second" }, }
            },
            new()
            {
                Id = 1,
                IsPrivate = true,
                LastModified = DateTime.Now.AddDays(1),
                Name = "Диалог с Васей",
                Users = new() { new() { }, new() { } },
                Messages = new() { new(null) { Id = 2, SelfMessage = false, Text = "third" }, new(null) { Id = 3, SelfMessage = true, Text = "fouth" }, }
            }
        };
    }

    #region Comands

    [ICommand]
    private async Task UpdateContacts()
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
    }

    #endregion
}
