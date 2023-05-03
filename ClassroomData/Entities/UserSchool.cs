using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomData.Entities;

public class UserSchool
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; }


    [ForeignKey("SchoolId")]
    public Guid SchoolId { get; set; }
    public School School { get; set; }

    public EUserSchool Type { get; set; }
}
