using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Extensions;
using TEKEVERChallenge.Services.Interfaces;
using TMDbLib.Objects.Search;

namespace TEKEVERChallenge.Services;

public class ShowService : IShowService
{
    private readonly TrackerContext _context;
    private readonly TMDbService _tmDbService;


    public ShowService(TrackerContext context, TMDbService tmDbService)
    {
        _context = context;
        _tmDbService = tmDbService;
    }
    
    public async Task<List<TvShow>> GetShows()
    {
        var shows = await _context.Shows.ToListAsync();
        return shows;
    }
    
    public async Task<TvShow?> GetShow(int id)
    {
        var show = await _context.Shows.FirstOrDefaultAsync(s => s.Id == id);
        return show;
    }
    
    public async Task<TvShow?> GetShowByName(string name)
    {
        var show = await _context.Shows.FirstOrDefaultAsync(s => s.Title == name);
        return show;
    }

    public async Task<TvShow> AddShowById(int id)
    {
        var bd = _tmDbService.GetTvShowAsync(id).Result;
        var show  = bd.MapToShow(_tmDbService);
        var cast = _tmDbService.GetTvShowCreditsAsync(id).Result.Cast;
        show.Characters = cast.Select(c => c.MapToCharacter(_context, show)).ToList();
        _context.Episodes.AddRange(show.Episodes);
        _context.Seasons.AddRange(show.Seasons);
        _context.Shows.Add(show);
        await _context.SaveChangesAsync();
        return show;
    }

    public TvShow GetTvShowFromDbWithSeasonsAndEpsByName(string name)
    {
        return _context.Shows.Where(x => x.Title == name).Include(x => x.Seasons).Include(x=> x.Episodes).FirstOrDefault()!;
    }
    
    public TvShow GetTvShowFromDbWithSeasonsAndEpsById(int id)
    {
        return _context.Shows.Where(x => x.Id == id).Include(x => x.Seasons).Include(x=> x.Episodes).FirstOrDefault()!;
    }
    
    public TvShow GetTvShowWithEverything(int id)
    {
        return _context.Shows.Where(x => x.Id == id).Include(x => x.Seasons).Include(x=> x.Episodes).Include(x=>x.Characters).FirstOrDefault()!;
    }
    
    
    
}

