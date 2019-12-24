using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
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
            // return some more user information so that photo can be disabled on nav bar.
            // you can create new user dto class but we are using UserForListDto now.
            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}