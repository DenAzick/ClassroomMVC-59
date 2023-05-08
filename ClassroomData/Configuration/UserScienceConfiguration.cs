using ClassroomData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomData.Configuration;

public class UserScienceConfiguration : IEntityTypeConfiguration<UserScience>
{
    public void Configure(EntityTypeBuilder<UserScience> builder)
    {
        builder.HasKey(s => new {s.UserId, s.ScienceId});
    }
}
