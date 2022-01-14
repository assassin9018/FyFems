using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UnitOfWork : IDisposable
{
    private readonly MyFemsDbContext _context;
    private bool disposedValue;

    #region Repositories
    private IRepository<Attachment>? _attachmentRepository;
    private IRepository<Dialog>? _dialogRepository;
    private IRepository<Image>? _imageRepository;
    private IRepository<Message>? _messageRepository;
    private IRepository<User>? _userRepository;

    public IRepository<Attachment> AttachmentRepository
        => _attachmentRepository ??= CreateRepository<Attachment>();
    public IRepository<Dialog> DialogRepository
        => _dialogRepository ??= CreateRepository<Dialog>();
    public IRepository<Image> ImageRepository
        => _imageRepository ??= CreateRepository<Image>();
    public IRepository<Message> MessageRepository
        => _messageRepository ??= CreateRepository<Message>();
    public IRepository<User> UserRepository
        => _userRepository ??= CreateRepository<User>();
    #endregion

    public UnitOfWork(MyFemsDbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public void Save()
        => _context.SaveChanges();

    public Task SaveAsync()
        => _context.SaveChangesAsync();

    private IRepository<T> CreateRepository<T>() where T : EntityBase
        => new PostgreRepository<T>(_context);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                _context.Dispose();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
