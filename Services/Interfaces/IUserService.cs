using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.Services.Interfaces;

public interface IUserService
{
    public List<Favorite> GetUserFavorites(User user);
    public List<RecommendationsDTO> GetRecommendations(IList<Favorite> favorites);

}