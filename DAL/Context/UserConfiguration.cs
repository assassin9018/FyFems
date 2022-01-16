using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasAlternateKey(x => x.Email)
            .HasName("EmailAltKey");

        builder
            .HasIndex(x => x.Phone)
            .IsUnique(true);

        const string tableName = $"\"{nameof(User.Surname)}s\"";

        builder
            .Property(x => x.FullName)
            .HasComputedColumnSql($"{tableName}.\"{nameof(User.Surname)}\" + ' ' + {tableName}.\"{nameof(User.Name)}\"", true);

        string ageSql = $"(f_person_age({tableName}.\"{nameof(User.BirthDate)}\"))";
        builder
            .Property(x => x.Age)
            .HasComputedColumnSql(ageSql, true);
    }
}
