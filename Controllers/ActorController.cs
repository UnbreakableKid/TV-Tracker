using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Services;
using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Extensions;
using TEKEVERChallenge.RequestHelpers;


namespace TEKEVERChallenge.Controllers;

public class ActorController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly TrackerContext _context;

    public ActorController(UserManager<User> userManager, TokenService tokenService, TrackerContext context) 
    {
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Gets a specific Actor.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The actor</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetActorById(int id)
    {
        var actor = await _context.Actors.Where(x => x.Id == id).Select(x  => new
        {
            x.Name,
            x.Gender
        }).FirstOrDefaultAsync();
        
        if (actor == null)
        {
            return NotFound();
        }
        
        return Ok(actor);
    }
    
    /// <summary>
    /// Get all actors.
    /// </summary>
    /// <param name="actorParams"></param>
    /// <returns>All actors</returns>
    [HttpGet]
    public async Task<ActionResult<PagedList<Actor>>> GetActors( [FromQuery] ActorParams actorParams)
    {
        var query = _context.Actors.Sort(actorParams.OrderBy).Search(actorParams.SearchTerm).AsQueryable();

        var actors = await PagedList<Actor>.ToPagedList(query, actorParams.PageNumber, actorParams.PageSize);
        
        Response.AddPaginationHeader(actors.MetaData);
        
        return Ok(actors);
    }
    
    /// <summary>
    /// Get characters by actor id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}/characters")]
    public async Task<IActionResult> GetCharacters(int id)
    {
        var characters = await _context.Characters.Where(x => x.actor.Id == id).Include(x=>x.tvShow).ToListAsync();
        return Ok(characters.Select(x=> new {x.character, x.tvShow.Title}));
    }

}