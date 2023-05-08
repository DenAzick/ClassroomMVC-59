
using System.ComponentModel.DataAnnotations;

namespace ClassroomMVC_59.Models;

public class CreateScienceDto
{
    [StringLength(50)]
    public string Name { get; set; }
    public string? Description { get; set; }
}
