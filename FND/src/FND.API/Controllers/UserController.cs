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

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FNDDBContext _context;
        public UserController(FNDDBContext fNDDBContext) 
        { 
            _context = fNDDBContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Users userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email== userObj.Email);
            if(user == null)
                return NotFound(new {Message = "User Not Found!"});

            if(!PasswordHasher.verifyPasswood(userObj.Password_hash, user.Password_hash))
            {
                return BadRequest(new { Message = "Password is incorrect" });
            }

            user.Token= CreateJwt(user);
            return Ok(new
            {
                Token = user.Token,
                Message = "Login Success"
            }) ;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users userObj)
        {
            if(userObj == null)
                return BadRequest();
            //check email exists
            if (await CheckEmailExist(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist!" });

            userObj.Password_hash = PasswordHasher.HashPassword(userObj.Password_hash);
            userObj.Token = "";
            await _context.Users.AddAsync(userObj);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "user registered!"
            });
        }

        private Task<bool> CheckEmailExist(string email)
            =>_context.Users.AnyAsync(x => x.Email == email);

        private string CreateJwt(Users user)
        {
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
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
