using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEKEVERChallenge.Entities
{
    public class TvShow
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        
        [Required]
        public DateTime ReleaseDate { get; set; }
        
        public string Genre { get; set; }
        public double Rating { get; set; }
        
        public IEnumerable<TvSeason> Seasons { get; set; }
        
        public int NumberOfSeasons => Seasons.Count();
        
        public IEnumerable<TvEpisode> Episodes { get; set; }
        
        public int NumberOfEpisodes => Episodes.Count();
        
        public IEnumerable<Character> Characters { get; set; }
        
    }
}