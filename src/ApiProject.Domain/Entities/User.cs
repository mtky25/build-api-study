namespace ApiProject.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } //navigation parameter
}
