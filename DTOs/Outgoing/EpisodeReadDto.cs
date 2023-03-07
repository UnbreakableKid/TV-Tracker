using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.DTOs;

public class EpisodeReadDto
{
   
    public string Name { get; set; }
    public string Description { get; set; }
    public int EpisodeNumber { get; set; }
    public DateTime AirDate { get; set; }
    public int Season { get; set; }
    public int TvShow { get; set; }
   
    
}