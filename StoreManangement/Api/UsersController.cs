using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManagement.Data.ViewModel.Authenticate;
using StoreManagement.Infrastructure;
using StoreManangement.Data.Data;
using StoreManangement.Service.Repository;
using StoreManangement.Service.UserService;

namespace StoreManangement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private IConfiguration _config;
        public UsersController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto user)
        {
            await _userService.CreateAsync(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginReturnViewModel>> Login(UserDto user)
        {
            var result = await _userService.AuthenticateLogin(user);
            if (result)
            {
                var userReturn = _userService.FindAsync(l => l.Email.Equals(user.Email)).Result.FirstOrDefault();
                var token = GenerateJsonWebToken(userReturn);
                var userResult = Mapper.Map<LoginReturnViewModel>(userReturn);
                userResult.Token = token;
                return userResult;
            }
            return BadRequest();
        }
        [HttpPost("Register")]
        public async Task<ActionResult<LoginReturnViewModel>> Register(UserDto user)
        {
            var check = _userService.FindAsync(l => l.Email.Equals(user.Email));
            if(check.Result.Count == 0) 
            {
                var passwordHash = Helper.GetMd5Hash(user.PasswordHash);
                user.PasswordHash = passwordHash;
                var userCreated = await _userService.CreateAsync(user);
                if (userCreated != null)
                {
                    return CreatedAtAction("GetUser", new { id = userCreated.Id }, userCreated);
                }
            }
            else
            {
                return BadRequest(new { message = "Email is existed" });
            }
            
            return BadRequest();
        }

        public string GenerateJsonWebToken(UserDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userInfo.RoleId == 1 ? "Admin" : "User"),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome" + userName;
        }

    }
}
