using Microsoft.AspNetCore.Identity;

namespace ClassroomData.Entities;

public  class User : IdentityUser<Guid>
{

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? PhotoUrl { get; set; }

    public List<UserSchool> Schools { get; set; }
    //public EUserStatus Status { get; set; }
}
