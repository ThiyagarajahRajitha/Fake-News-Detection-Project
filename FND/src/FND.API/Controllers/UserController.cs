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
    public class UserController : ControllerBase
    {
        private readonly FNDDBContext _context;
        private readonly UserService userService;
        public UserController(FNDDBContext fNDDBContext) 
        {
            _context = fNDDBContext;
            userService = new UserService(fNDDBContext);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email== loginRequest.Username);
            if(user == null)
                return NotFound(new {Message = "User Not Found!"});

            if(!PasswordHasher.verifyPasswood(loginRequest.Password, user.Password_hash))
            {
                return BadRequest(new { Message = "Password is incorrect" });
            }
            if(user.Status == 0)
            {
                return BadRequest(new { Message = "Your account approval is pending" });
            }

            user.Token= CreateJwt(user);
            return Ok(new
            {
                Token = user.Token,
                Message = "Login Success"
            }) ;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] SignUpRequestDto signUpRequest)
        {
            if(signUpRequest == null)
                return BadRequest();
            //check email exists
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Email == signUpRequest.Email);
            if (await CheckUserEmailExist(signUpRequest.Email))
                return BadRequest(new { Message = "Email Already Exist!" });

            //signUpRequest.Password = PasswordHasher.HashPassword(signUpRequest.Password);
            ////users.Token = "";
            //Users user = new Users()
            //{
            //    Name = signUpRequest.Name,
            //    Email = signUpRequest.Email,
            //    Password_hash = signUpRequest.Password,
            //    Created_at = DateTime.UtcNow,
            //    Token = "",
            //    Role = "Publisher",
            //    Status = 0
            //};


            //await _context.Users.AddAsync(user);
            //await _context.SaveChangesAsync();
            await userService.CreateUser(signUpRequest);
            return Ok(new
            {
                Message = "user registered!"
            });
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

        //private async Task<IActionResult> UpdateModerator([FromRoute] string email)
        //{
        //    var updatedMod = new Moderator { Username = email, IsAccepted = true };
        //    _context.Attach(updatedMod);
        //    _context.Entry(updatedMod).Property(r => r.IsAccepted).IsModified = true;
        //    _context.SaveChanges();
        //    return Ok();

        //}
        private Task<bool> CheckUserEmailExist(string email)
            =>_context.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckModeratorEmailExist(string email)
            => _context.Moderators.AnyAsync(x => x.Username == email);
        private string CreateJwt(Users user)
        {
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString())
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenhandler.CreateToken(tokenDescriptor);
            return jwtTokenhandler.WriteToken(token);
        }
    }
}
