using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.DTOs;

public class ShowReadDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<CharacterReadDto> Characters { get; set; }
    
    public IEnumerable<SeasonReadDto> Seasons { get; set; }
    public  string Genre { get; set; }
    public DateTime AirDate { get; set; }
    
    
}