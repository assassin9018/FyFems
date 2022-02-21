using ClientLocalDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientLocalDAL.Context;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(x => x.Phone)
            .IsUnique(true);

        const string tableName = $"\"{nameof(User)}s\"";
    }
}
