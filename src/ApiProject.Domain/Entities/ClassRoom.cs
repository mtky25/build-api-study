namespace ApiProject.Domain.Entities
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; } //navigation parameter
    }
}
