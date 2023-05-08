using System.ComponentModel.DataAnnotations;

namespace ClassroomMVC_59.Models;

public class UpdateSchoolDto
{
    [MaxLength(50)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Photo { get; set; }
}
