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

        //builder
        //    .Property(x => x.FullName)
        //    .HasComputedColumnSql($"\"{nameof(User.Surname)}\" + ' ' + \"{nameof(User.Name)}\"", true);

        //const string ageSql = $"(YEAR(GETDATE()) - YEAR(\"{nameof(User.BirthDate)}\")) - (DATE_FORMAT(GETDATE(), '%m%d') < DATE_FORMAT(\"{nameof(User.BirthDate)}\", '%m%d'))";
        //builder
        //    .Property(x => x.Age)
        //    .HasComputedColumnSql(ageSql, true);
    }
}
