using ClassroomData.Context;
using ClassroomData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomData.Configuration;

public class UserSchoolConfiguration : IEntityTypeConfiguration<UserSchool>
{
    public void Configure(EntityTypeBuilder<UserSchool> builder)
    {
        

        builder.ToTable(nameof(UserSchool));

        builder.HasKey(userSchool => new {userSchool.UserId, userSchool.SchoolId});

        builder
            .HasOne(userSchool => userSchool.User)
            .WithMany(user => user.Schools)
            .HasForeignKey(us => us.UserId);

        builder
            .HasOne(us => us.School)
            .WithMany(s => s.Users)
            .HasForeignKey(us => us.SchoolId);
    }

}
