using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomData.Entities;


[Table("schools")]
public class School
{
    [Key]
    public Guid Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("photo_url_school")]
    public string? PhotoUrlSchool { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    public List<UserSchool> Users { get; set; }
}
