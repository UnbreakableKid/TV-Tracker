using TEKEVERChallenge.Data;
using TEKEVERChallenge.Entities;
using TMDbLib.Client;
using TMDbLib.Objects.Search;
using Cast = TMDbLib.Objects.TvShows.Cast;

namespace TEKEVERChallenge.Extensions;

public static class TMDbExtensions
{
    public static TvShow MapToShow(this TMDbLib.Objects.TvShows.TvShow show, TMDbClient client)
    {
        List<TvSeason> seasons = new List<TvSeason>();

        var tvShow = new TvShow();

        foreach (var season in show.Seasons)
        {
            var tvSeason = client.GetTvSeasonAsync(show.Id, season.SeasonNumber).Result;
            var tvsSeason = tvSeason.MapToSeason(tvShow, show, client);
            seasons.Add(tvsSeason);
        }

        tvShow.Description = show.Overview;
        tvShow.Genre = show.Genres.Select(g => g.Name).ToList().First();
        tvShow.ImageUrl = show.PosterPath;
        tvShow.Title = show.Name;
        tvShow.ReleaseDate = show.FirstAirDate ?? DateTime.MinValue;
        tvShow.Rating = show.VoteAverage;
        tvShow.Seasons = seasons;
        tvShow.Episodes = seasons.SelectMany(s => s.TvEpisodes).ToList();
      
        return tvShow;
    }
    
    public static TvSeason MapToSeason(this TMDbLib.Objects.TvShows.TvSeason season, TvShow tvShow ,TMDbLib.Objects.TvShows.TvShow show, TMDbClient client)
    {
        var tvSeason = new TvSeason();
        List<TvEpisode> episodes = new List<TvEpisode>();
        
        foreach (var episode in season.Episodes)
        {
            var tvsEpisode = episode.MapToEpisode(tvShow, tvSeason, client);
            episodes.Add(tvsEpisode);
        }

        tvSeason.TvEpisodes = episodes;
        tvSeason.SeasonNumber = season.SeasonNumber;
        tvSeason.TvShow = tvShow;
        return tvSeason;
    }
    
    
    public static TvEpisode MapToEpisode(this TvSeasonEpisode episode, TvShow show, TvSeason season, TMDbClient client)
    {
        
        return new TvEpisode()
        {
            Name = episode.Name,
            Description = episode.Overview,
            EpisodeNumber = episode.EpisodeNumber,
            TvShow = show,
            Season = season,
            AirDate = episode.AirDate ?? DateTime.MinValue,
        };
    }
    
    public static Character MapToCharacter(this Cast cast, TrackerContext context, TvShow show)
    {
        var actor = context.Actors.FirstOrDefault(a => a.Name == cast.Name);
        
        if(actor == null)
        {
            actor = new Actor()
            {
                Name = cast.Name,
                Gender = cast.Gender.ToString(),
                
            };
            context.Actors.Add(actor);
        }
        
        return new Character()
        {
            character = cast.Character,
            tvShow = show,
            actor = actor
        };
    }
}