using TEKEVERChallenge.Entities;
using TMDbLib.Objects.Search;

namespace TEKEVERChallenge.Services.Interfaces;

public interface IShowService
{
    public Task<List<TvShow>> GetShows();
    public Task<TvShow?> GetShow(int id);
    public Task<TvShow?> GetShowByName(string name);
    public Task<TvShow> AddShowById(int id);
    
    public TvShow GetTvShowFromDbWithSeasonsAndEpsByName(string name);

    public TvShow GetTvShowFromDbWithSeasonsAndEpsById(int id);

    public TvShow GetTvShowWithEverything(int id);


}