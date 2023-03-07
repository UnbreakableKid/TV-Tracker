namespace TEKEVERChallenge.Entities;

public class Favorite
{
    public int Id { get; set; }
    public User User { get; set; }
    public TvShow TvShow { get; set; }
}