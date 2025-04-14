namespace error_repro;

public class Parent
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Child> Children { get; set; } = new List<Child>();
}