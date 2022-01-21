using ClientLocalDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientLocalDAL.Context;

#nullable disable
public class SqLiteDbContext : DbContext
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Dialog> Dialogs { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }


    public SqLiteDbContext(DbContextOptions options) : base(options)
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
    }
}