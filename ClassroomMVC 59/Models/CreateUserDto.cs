using System.ComponentModel.DataAnnotations;

namespace ClassroomMVC_59.Models;

public class CreateUserDto
{

    [Required]
    [MinLength(3)]
    public string Firtname { get; set; }

    [Required]
    [MinLength(3)]
    public string Lastname { get; set; }
    public string Username { get; set; }

    [MinLength(6)]
    public string Password { get; set; }

    [RegularExpression("^[0-9]{9}$", ErrorMessage = "wrong phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    public IFormFile? Photo { get; set; }


}
