namespace TEKEVERChallenge.Entities;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public IEnumerable<Character> Characters { get; set; }
}