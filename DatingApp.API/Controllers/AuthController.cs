using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //validate request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already Exists.");
            }
            var usercreate = new User
            {
                Username = userForRegisterDto.Username
            };
            var createduser = await _repo.Register(usercreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var loginuser = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (loginuser == null)
                return Unauthorized();

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier ,loginuser.Id.ToString()),
            new Claim(ClaimTypes.Name , loginuser.Username)
        };

            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokendescriptor=new SecurityTokenDescriptor
            {
              Subject=new ClaimsIdentity(claims),
              Expires= DateTime.Now.AddDays(1),
              SigningCredentials=creds
            };

            var tokenhandler=new JwtSecurityTokenHandler();
            var token= tokenhandler.CreateToken(tokendescriptor);

            return Ok(new 
             {
              token=tokenhandler.WriteToken(token)
             });
        }
    }
}