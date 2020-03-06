using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManagement.Data.ViewModel.Authenticate;
using StoreManangement.Models;
using StoreManangement.Service.Repository;
using StoreManangement.Service.UserService;

namespace StoreManangement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private IConfiguration _config;

        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }
        
        [HttpPost]
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
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        [Authorize]
        [HttpGet("me")]
        public string me()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return userName;
        }

        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }
    }
}