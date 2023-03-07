using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Extensions;
using TEKEVERChallenge.RequestHelpers;
using TEKEVERChallenge.Services;
using TEKEVERChallenge.Services.Interfaces;

namespace TEKEVERChallenge.Controllers
{
    public class ShowController : BaseApiController
    {
        private readonly TrackerContext _context;
        private readonly IShowService _showService;
        private readonly TMDbService _tmDbService;


        public ShowController(TrackerContext context, IShowService showService, TMDbService tmDbService)
        {
            _context = context;
            _showService = showService;
            _tmDbService = tmDbService;
        }

        /// <summary>
        /// Get all the shows in the database
        /// </summary>
        /// <param name="showParams"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<ShowReadDto>>> GetShows([FromQuery] ShowParams showParams)
        {
            var queryable = _context.Shows.Select(x=> _showService.GetTvShowWithEverything(x.Id));

            var showReadDtos = queryable.Select(x => x.MapShowToDto(_context)).ToList();
            var query = showReadDtos.Sort(showParams.OrderBy).Search(showParams.SearchTerm).Filter(showParams.Genres);

            var shows = await PagedList<ShowReadDto>.ToPagedList(query, showParams.PageNumber, showParams.PageSize);

            Response.AddPaginationHeader(shows.MetaData);

            return shows;
        }


        /// <summary>
        /// Gets a show info by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TvShow>> GetShow(int id)
        {
            var show = _showService.GetTvShowFromDbWithSeasonsAndEpsById(id);
            if (show == null)
            {
                return NotFound();
            }
            
            return Ok(show.MapShowToDto(_context));
        }

        /// <summary>
        /// Get the available show filters
        /// </summary>
        /// <returns></returns>
        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var genres = await _context.Shows.Select(p => p.Genre).Distinct().ToListAsync();
            
            return Ok(new { genres });

        }
        
        
        /// <summary>
        /// Add a new movie to the DB by name.
        /// </summary>
        /// <param name="showDto"></param>
        /// <returns>The created show</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<TvShow>> AddShow(CreateShowDto showDto)
        {
            var res = _tmDbService.SearchTvShowAsync(showDto.Title).Result.Results.FirstOrDefault();
            if (res == null)
            {
                return BadRequest("No results found");
            }
            
            var exists = _context.Shows.Any(p => p.Title == res.Name && p.ReleaseDate == res.FirstAirDate);
            if (exists)
            {
                return BadRequest("Show already exists");
            }

            var showId = res.Id;
            
            var show = await _showService.AddShowById(showId);

            var showReadDto = show.MapShowToDto(_context);

            return Ok(showReadDto);
        }

    }
}