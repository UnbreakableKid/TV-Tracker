using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;
using TvSeason = TMDbLib.Objects.TvShows.TvSeason;

namespace TEKEVERChallenge.Extensions
{
    public static class MovieExtensions
    {
        public static IQueryable<TvShow> Sort(this IQueryable<TvShow> query, string? orderBy)
        {
            query = orderBy switch
            {
                "title" => query.OrderBy(p => p.Title),
                "genre" => query.OrderBy(p => p.Genre),
                "releaseDesc" => query.OrderByDescending(p => p.ReleaseDate),
                _ => query.OrderBy(p => p.Title)

            };

            return query;
        }
        
        public static IEnumerable<ShowReadDto> Sort(this IEnumerable<ShowReadDto> query, string? orderBy)
        {
            query = orderBy switch
            {
                "title" => query.OrderBy(p => p.Title),
                "genre" => query.OrderBy(p => p.Genre),
                "releaseDesc" => query.OrderByDescending(p => p.AirDate),
                _ => query.OrderBy(p => p.Title)

            };

            return query;
        }

        public static IQueryable<TvShow> Search(this IQueryable<TvShow> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }
            var lowerCased = searchTerm.Trim().ToLowerInvariant();

            return query.Where(p => p.Title.ToLower().Contains(lowerCased));
        }
        
        public static IEnumerable<ShowReadDto> Search(this IEnumerable<ShowReadDto> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }
            var lowerCased = searchTerm.Trim().ToLowerInvariant();

            return query.Where(p => p.Title.ToLower().Contains(lowerCased));
        }

        public static IQueryable<TvShow> Filter(this IQueryable<TvShow> query, string? genres)
        {
            var genreList = new List<string>();
            var typeList = new List<string>();

            if (!string.IsNullOrEmpty(genres))
            {
                genreList.AddRange(genres.ToLower().Split(',').ToList());
            }

            query = query.Where(p => genreList.Count == 0 || genreList.Contains(p.Genre.ToLower()));

            return query;

        }

        
        public static IEnumerable<ShowReadDto> Filter(this IEnumerable<ShowReadDto> query, string? genres)
        {
            var genreList = new List<string>();
            var typeList = new List<string>();

            if (!string.IsNullOrEmpty(genres))
            {
                genreList.AddRange(genres.ToLower().Split(',').ToList());
            }

            query = query.Where(p => genreList.Count == 0 || genreList.Contains(p.Genre.ToLower()));

            return query;

        }
        

        public static List<CharacterReadDto> MapShowCaractersToDto(this TvShow show)
        {
            
            return show.Characters.Select(p => new CharacterReadDto()
            {
                ActorName = p.actor.Name,
                Name = p.character,
                
            }).ToList();
        }
        
        public static ShowReadDto MapShowToDto(this TvShow show, TrackerContext _context)
        {
            
          
            if (show.Characters== null || show.Characters.Any(x => x.actor == null))
            {
                show.Characters = _context.Characters.Where(x => x.tvShow.Id == show.Id).Include(x => x.actor).ToList();
            }
            
            
            return new ShowReadDto()
            {
                Characters = show.MapShowCaractersToDto(),
                Description = show.Description,
                Genre = show.Genre,
                Title = show.Title,
                Seasons = show.MapSeasonsToDto(),
                AirDate = show.ReleaseDate
            };
        }
        
        public static List<SeasonReadDto> MapSeasonsToDto(this TvShow show)
        {
            return show.Seasons.Select(s => new SeasonReadDto()
            {
                SeasonNumber = s.SeasonNumber,
                TvShow = s.TvShow.Title,
                TvEpisodes = s.MapEpisodeReadDtos()
            }).ToList();
        }
        
        public static List<EpisodeReadDto> MapEpisodeReadDtos(this TEKEVERChallenge.Entities.TvSeason season)
        {
            return season.TvEpisodes.Select(e => new EpisodeReadDto()
            {
                EpisodeNumber = e.EpisodeNumber,
                Name = e.Name,
                Description = e.Description,
                AirDate = e.AirDate,
                TvShow = season.TvShowId,
                Season = season.SeasonNumber
            }).ToList();
        }
    }
}