using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using newProjectApp.API.Data;
using newProjectApp.API.DTOS;
using newProjectApp.API.Modles;
using System.Security.Cryptography;
using System.Text;

namespace newProjectApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository repo, IConfiguration configuration)
        {
            _configuration = configuration;
            _repo = repo;

        }

        [HttpPost("register")]

        public async Task<IActionResult> register(UserRegisterDTO userRegisterDTO)
        {
            //validation
            userRegisterDTO.username = userRegisterDTO.username.ToLower();
            if (await _repo.UserExists(userRegisterDTO.username))
                return BadRequest("هذا المستخدم مسجل من قبل ");
            var userToCreate = new User
            {
                Username = userRegisterDTO.username

            };
            var CreatedUser = await _repo.Register(userToCreate, userRegisterDTO.password);

            return StatusCode(201);


        }

        [HttpPost("login")]

        public async Task<IActionResult> login(UserForLoginDTO userForLoginDTO)
        {

            var userfromrepo = await _repo.Login(userForLoginDTO.username.ToLower(), userForLoginDTO.password);
            if (userfromrepo == null) return Unauthorized();
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userfromrepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userfromrepo.Username)

            };
          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512); 
            var tokenDescriptor=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds

            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new{
                token=tokenHandler.WriteToken(token)
            } );



        }

      
    }
}