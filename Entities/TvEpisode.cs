using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEKEVERChallenge.Entities;

public class TvEpisode
{
    [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int EpisodeNumber { get; set; }
    public DateTime AirDate { get; set; }
    
    public int SeasonId { get; set; }
    public TvSeason Season { get; set; }
    public int TvShowId { get; set; }
    public TvShow TvShow { get; set; }
    
}