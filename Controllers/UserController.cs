using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Services;
using TEKEVERChallenge.Services.Interfaces;

namespace TEKEVERChallenge.Controllers;

public class UserController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly TrackerContext _context;
    private readonly IShowService _showService;
    private readonly IUserService _userService;

    public UserController(UserManager<User> userManager, TokenService tokenService, TrackerContext context, IShowService showService, IUserService userService)
    {
       _context = context;
       _showService = showService;
       _userService = userService;
       _userManager = userManager;
    }
    
    /// <summary>
    /// Get all the users in the database
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        return users.Select(x => new UserReadDTO()
        {
            Id = x.Id,
            Username = x.UserName,
        }).ToList();
    }
    
    /// <summary>
    /// Get a user by their Username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}", Name = "GetUserByUsername")]
    public async Task<ActionResult<UserReadDTO>> GetUserByUsername(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return new UserReadDTO()
        {
            Id = user.Id,
            Username = user.UserName
        };
    }
    
    /// <summary>
    /// Get a User's favorites
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}/favorites", Name = "GetUserFavorites")]
    public async Task<ActionResult<IEnumerable<TvShow>>> GetUserFavorites(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        var favorites = _context.Favorites.Where(f => f.User.Id == user.Id).Include(f => f.TvShow).ToList();

        return Ok(favorites.Select(x=>new {x.TvShow.Id, x.TvShow.Title}));
    }
    
    
    
    /// <summary>
    /// Add a Show to a user's favorites
    /// </summary>
    /// <param name="username"></param>
    /// <param name="showName"></param>
    /// <returns></returns>
    [HttpPost("{username}/favorites", Name = "AddFavorite")]
    public async Task<ActionResult<TvShow>> AddFavorite(string username, [FromBody] string showName)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var show = _showService.GetTvShowFromDbWithSeasonsAndEpsByName(showName);

        if (show == null)
        {
            return NotFound("Show not found");
        }
        
        if(_context.Favorites.Any(x => x.User.Id == user.Id && x.TvShow.Id == show.Id))
        {
            return BadRequest("Show already added to favorites");
        }
        
        var favorite = new Favorite()
        {
            User = user,
            TvShow = show
        };
        
        _context.Favorites.Add(favorite);

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    /// <summary>
    /// Remove a show from the user's favorites
    /// </summary>
    /// <param name="username"></param>
    /// <param name="showId"></param>
    /// <returns></returns>
    [HttpDelete("{username}/favorites/{showId}", Name = "RemoveFavorite")]
    public async Task<ActionResult<TvShow>> RemoveFavorite(string username, int showId)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound("User not found");
        }
        
        var show = _showService.GetTvShowFromDbWithSeasonsAndEpsById(showId);

        if (show == null)
        {
            return NotFound("Show not found");
        }
        
        var favorite = _context.Favorites.FirstOrDefault(x => x.User.Id == user.Id && x.TvShow.Id == show.Id);

        if (favorite == null)
        {
            return NotFound("Show not found");
        }
        
        _context.Favorites.Remove(favorite);

        await _context.SaveChangesAsync();

        return Ok(favorite.TvShow.Title);
    }
    
    
    /// <summary>
    /// Get a User's recommendations (2) based on their favorites
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}/recommendation")]
    public async Task<ActionResult<IEnumerable<RecommendationsDTO>>> GetUserRecommendations(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        var favorites = _userService.GetUserFavorites(user);
        
        var recommendations = _userService.GetRecommendations(favorites);

        return Ok(recommendations);
    }
}