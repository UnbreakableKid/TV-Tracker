namespace TEKEVERChallenge.DTOs;

public class RecommendationsDTO
{
    public string BasedOn { get; set; }
    
    public IEnumerable<Recommendations> Recommendations { get; set; }
}

public class Recommendations
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? AirDate { get; set; }
}