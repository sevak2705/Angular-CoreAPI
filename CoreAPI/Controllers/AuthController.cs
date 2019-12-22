using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreAPI.Dto;
using CoreAPI.Models;
using CoreAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo; 
        private IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registration(UserForRegistrationDto userDto)
        {
            //check is user exist
           if(await _repo.UserExist(userDto.UserName.ToLower()))
           {
               return BadRequest("User all ready exist");
           }
           var user = new User();
           user.UserName = userDto.UserName;
          
          var createdUser = await _repo.Register(user, userDto.Password);
          return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto obj)
        {
            var userFromRepo = await _repo.Login(obj.UserName.ToLower(), obj.Password);
            if (userFromRepo == null) {
                return Unauthorized();
            }
            var claim = new[] {
            new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
            new Claim(ClaimTypes.Name,userFromRepo.UserName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}