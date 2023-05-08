namespace ClassroomData.Entities;

public class JoinScienceRequest
{
    public Guid Id { get; set; }
    public Guid ScienceId { get; set; }
    public Guid ToUserId { get; set; }
    public Guid FromUserId { get; set; }
    public bool IsJoined { get; set; }

}
