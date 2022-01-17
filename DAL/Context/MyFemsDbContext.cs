using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Context;

public class MyFemsDbContext : DbContext
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Dialog> Dialogs { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactRequest> ContactRequests { get; set; }

    public MyFemsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
#if DEBUG
        var logLevel = LogLevel.Trace;
#else
            var logLevel = LogLevel.None;
#endif
        builder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, minimumLevel: logLevel);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new MessageConfiguration());
    }

    #region Stored functions
    #endregion
    #region Stored procedures
    public IQueryable<Message> GetDialogMessages(int dialogId, int messageMinId) 
        => FromExpression(() => GetDialogMessages(dialogId, messageMinId));
    #endregion
}