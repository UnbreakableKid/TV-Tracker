namespace TEKEVERChallenge.Entities;

public class Character
{
    public int Id { get; set; }
    public string character { get; set; }
    public Actor actor { get; set; }
    public TvShow tvShow { get; set; }
}