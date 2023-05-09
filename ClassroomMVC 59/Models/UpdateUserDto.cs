
using System.ComponentModel.DataAnnotations;

namespace ClassroomMVC_59.Models;

public class UpdateUserDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string Username { get; set; }

    [MinLength(6)]
    public string Password { get; set; }

    [RegularExpression("^[0-9]{9}$", ErrorMessage = "wrong phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    public IFormFile? Photo { get; set; }
}
