using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClassroomData.Entities;

public  class User : IdentityUser<Guid>
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? PhotoUrl { get; set; }

    public List<UserSchool> Schools { get; set; }
    //public EUserStatus Status { get; set; }
}
