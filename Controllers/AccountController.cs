using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TEKEVERChallenge.Data;
using TEKEVERChallenge.DTOs;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Services;

namespace TEKEVERChallenge.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly TrackerContext _context;

        public AccountController(UserManager<User> userManager, TokenService tokenService, TrackerContext context)
        {
            _tokenService = tokenService;
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Login an Account.
        /// </summary>
        /// <param name="loginDto" type="LoginDto"></param>
        /// <returns>The user email and a generated JWT token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
            });
        }
        
        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="registerDto">The user to register.</param>
        /// <returns>User ID</returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return Created("/api/Account/login", user.Id);
        }

        /// <summary>
        /// Gets the current user. Useful to regenerate a new token.
        /// </summary>
        /// <returns>User email and a new token</returns>
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
            };
        }

    }
}