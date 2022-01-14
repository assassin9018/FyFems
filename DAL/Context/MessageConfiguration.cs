using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        //builder
        //    .Property(x => x.AttachmentsCount)
        //    .HasComputedColumnSql<int>($"cardinality(\"{nameof(Message.Attachments)}\")", true);

        //builder
        //    .Property(x => x.ImagesCount)
        //    .HasComputedColumnSql<int>($"cardinality(\"{nameof(Message.Images)}\")", true);
    }
}
