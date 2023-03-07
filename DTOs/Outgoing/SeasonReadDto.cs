using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.DTOs;

public class SeasonReadDto
{
    public int SeasonNumber { get; set; }
    public string TvShow { get; set; }
    public ICollection<EpisodeReadDto> TvEpisodes { get; set; }
    
}