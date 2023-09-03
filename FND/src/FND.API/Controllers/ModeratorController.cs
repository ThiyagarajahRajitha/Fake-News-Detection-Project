using FND.API.Data;
using FND.API.Entities;
using FND.API.Helpers;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Services;

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeratorController : ControllerBase
    {
        private readonly FNDDBContext _context;
        private readonly UserService userService;
        public ModeratorController(FNDDBContext fNDDBContext) 
        {
            _context = fNDDBContext;
            userService = new UserService(fNDDBContext);
        }

        [HttpPost("CreateModerator")]
        public async Task<ActionResult<Moderator>> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            if (createModeratorDto == null)
                return BadRequest();

            if (await CheckModeratorEmailExist(createModeratorDto.Email))
                return BadRequest(new { Message = "Email Already Exist!" });

            var moderator  = await userService.CreateModerator(createModeratorDto);
            return Ok();
        }

        [HttpPost("ValidateModerator")]
        public async Task<IActionResult> ValidateModerator([FromQuery(Name = "username")] string userName, [FromQuery(Name = "inviteCode")] Guid inviteCode)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            bool isvalid= await userService.ValidateModerator(userName, inviteCode);
            if (isvalid)
                return Ok();
            else 
                return BadRequest();

        }

        [HttpPost("RegisterModerator")]
        public async Task<IActionResult> RegisterModerator([FromBody] ModeratorSignUpRequestDto moderatorSignUp)
        {
            if (moderatorSignUp == null)
                return BadRequest();
            //check email exists
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Email == signUpRequest.Email);
            if (await CheckModeratorEmailExist(moderatorSignUp.Email))
            {
                bool isvalid = await userService.ValidateModerator(moderatorSignUp.Email, moderatorSignUp.InviteCode);
                if (isvalid)
                {
                    moderatorSignUp.Password = PasswordHasher.HashPassword(moderatorSignUp.Password);
                    //users.Token = "";
                    Users user = new Users()
                    {
                        Name = moderatorSignUp.Name,
                        Email = moderatorSignUp.Email,
                        Password_hash = moderatorSignUp.Password,
                        Created_at = DateTime.UtcNow,
                        Token = "",
                        Role = "Moderator",
                        Status = 1
                    };

                    if (await CheckUserEmailExist(user.Email))
                    {
                        return BadRequest(new
                        {
                            Message = "User Already Exist!"
                        });
                    }
                    await _context.Users.AddAsync(user);
                    await userService.DeleteModerator(moderatorSignUp.Email);
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = "Registered as Moderator!"
                    });
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound(new
                {
                    Message = "No Username found!"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetModerators([FromQuery(Name = "Pending")] bool IsPendingOnly)
        {
            var result = await userService.GetModerators(IsPendingOnly);
            return result;
        }

        private Task<bool> CheckUserEmailExist(string email)
            =>_context.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckModeratorEmailExist(string email)
            => _context.Moderators.AnyAsync(x => x.Username == email);
    }
}
