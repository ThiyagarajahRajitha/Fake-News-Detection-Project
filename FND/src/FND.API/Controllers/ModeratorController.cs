using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Helpers;
using FND.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeratorController : ControllerBase
    {
        private readonly FNDDBContext _context;
        private readonly ModeratorService moderatorService;
        private readonly UserService userService;
        public ModeratorController(FNDDBContext fNDDBContext) 
        {
            _context = fNDDBContext;
            moderatorService = new ModeratorService(fNDDBContext);
            userService = new UserService(fNDDBContext);
        }

        [Authorize]
        [HttpPost("CreateModerator")]
        public async Task<ActionResult<Moderator>> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            if (createModeratorDto == null)
                return BadRequest();

            if (await CheckModeratorEmailExist(createModeratorDto.Email) || await CheckUserEmailExist(createModeratorDto.Email))
                return BadRequest(new { Message = "Email Already Exist!" });

            var moderator  = await moderatorService.CreateModerator(createModeratorDto);
            return Ok();
        }

        [Authorize]
        [HttpPost("Reinvite")]
        public async Task<ActionResult<Moderator>> ReinviteModerator(int id)
        {
            var rslt = await moderatorService.ReInviteModerator(id);
            if(rslt == true)
                return Ok();
            return BadRequest();
        }

        [HttpPost("ValidateModerator")]
        public async Task<IActionResult> ValidateModerator([FromQuery(Name = "username")] string userName, [FromQuery(Name = "inviteCode")] Guid inviteCode)
        {
            if (userName == null || userName.IsNullOrEmpty())
                return BadRequest();
            //var newsList = await _fNDDBContext.News.ToListAsync();
            bool isvalid= await moderatorService.ValidateModerator(userName, inviteCode);
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
            if (await CheckModeratorEmailExist(moderatorSignUp.Email))
            {
                bool isvalid = await moderatorService.ValidateModerator(moderatorSignUp.Email, moderatorSignUp.InviteCode);
                if (isvalid)
                {
                    moderatorSignUp.Password = PasswordHasher.HashPassword(moderatorSignUp.Password);
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
                    await moderatorService.DeleteModerator(moderatorSignUp.Email);
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetModerators([FromQuery(Name = "Pending")] bool IsPendingOnly)
        {
            var result = await userService.GetModerators(IsPendingOnly);
            return result;
        }

        

        //[Authorize]
        //[Route("DeleteModeratorById")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteModeratorById([FromRoute] int id)
        {
            bool rslt = await moderatorService.DeleteModeratorById(id);
            if (rslt == true)
                return Ok();
            else
                return BadRequest(rslt);
        }

        [Authorize]
        [Route("GetReviewRequestCountByModerator")]
        [HttpGet]
        public async Task<ActionResult<List<ReviewRequestCountByModeratorDashboardresultDto>>> GetReviewRequestCountByModerator(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            if (userId == null)
                return BadRequest();

            var reviewRequestCountByModerator = await userService.GetReviewRequestCountByModerator(userId, fromDate, toDate);
            return reviewRequestCountByModerator;
        }


        private Task<bool> CheckUserEmailExist(string email)
            =>_context.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckModeratorEmailExist(string email)
            => _context.Moderators.AnyAsync(x => x.Username == email);
    }
}
