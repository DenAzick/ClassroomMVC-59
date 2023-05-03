
using System.ComponentModel.DataAnnotations;

namespace ClassroomMVC_59.Models;

public class SignInDto
{
    [Required]
    public string Username { get; set; }

    [Required] 
    public string Password { get;set; }
}
