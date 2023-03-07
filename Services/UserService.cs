using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Extensions;
using TEKEVERChallenge.Services.Interfaces;

namespace TEKEVERChallenge.Services;

public class UserService : IUserService
{
    private readonly TrackerContext _context;
    private readonly TMDbService _tmDbService;


    public UserService(TrackerContext context, TMDbService tmDbService)
    {
        _context = context;
        _tmDbService = tmDbService;
    }

    public List<Favorite> GetUserFavorites(User user)
    {
        return  _context.Favorites.Where(f => f.User.Id == user.Id).Include(f => f.TvShow).ToList();
    }

    public List<RecommendationsDTO> GetRecommendations(IList<Favorite> favorites)
    {
        var tmdbIds = favorites.Select(f => _tmDbService.SearchTvShowAsync(f.TvShow.Title).Result.Results[0]).ToList();
        
        var recommendations = new List<RecommendationsDTO>();
        
        foreach (var tmdbId in tmdbIds)
        {
            var z = _tmDbService.GetTvShowAsync(tmdbId.Id).Result;
            var recommendation = _tmDbService.GetTvShowRecommendationsAsync(z.Id).Result.Results;
            
            var rec = new List<Recommendations>();
            for (int i = 0; i < 2; i++) //2 for now
            {
                rec.Add(new Recommendations()
                {
                    AirDate = recommendation[i].FirstAirDate,
                    Title = recommendation[i].Name,
                    Description = recommendation[i].Overview
                });
            }
            recommendations.Add(new RecommendationsDTO()
            {
                BasedOn = tmdbId.Name,
                Recommendations = rec
            });
        }
        return recommendations;
    }
}