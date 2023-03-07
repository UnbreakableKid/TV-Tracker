using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEKEVERChallenge.Entities;

public class TvSeason
{
    [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int SeasonNumber { get; set; }
    public int TvShowId { get; set; }
    public TvShow TvShow { get; set; }
    public ICollection<TvEpisode> TvEpisodes { get; set; }
}