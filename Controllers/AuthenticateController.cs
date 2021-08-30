using Library.API.Entities;
using Library.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public RoleManager<Role> RoleManager { get; }

        public UserManager<User> UserManager { get; }

        public AuthenticateController(UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            Configuration = configuration;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        [HttpPost("token", Name = nameof(GenerateToken))]
        public IActionResult GenerateToken(LoginUser loginUser)
        {
            if (loginUser.UserName != "demouser" || loginUser.Password != "demopassword")
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.UserName)
            };

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    issuer: tokenConfigSection["Issuer"],
                    audience: tokenConfigSection["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signCredential
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });


}

        [HttpPost("register", Name = nameof(AddUserAsync))]
        public async Task<IActionResult> AddUserAsync(RegisterUser registerUser)
        {
            var user = new User
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                BirthDate = registerUser.BirthDate
            };

            IdentityResult result = await UserManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await AddUserToRoleAsync(user, "Administrator");
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }

        }

        [HttpPost("token2", Name = nameof(GenerateTokenAsync))]
        public async Task<IActionResult> GenerateTokenAsync(LoginUser loginUser)
        {
            var user = await UserManager.FindByNameAsync(loginUser.UserName);

            if(user == null)
            {
                return Unauthorized();
            }

            var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

            if(result != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);

            foreach(var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(userClaims);

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    issuer: tokenConfigSection["Issuer"],
                    audience: tokenConfigSection["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(3),
                    signingCredentials: signCredential
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });


        }

        private async Task AddUserToRoleAsync(User user, String roleName)
        {
            if(user == null || string.IsNullOrWhiteSpace(roleName))
            {
                return;
            }

            bool isRoleExist = await RoleManager.RoleExistsAsync(roleName);

            if (!isRoleExist)
            {
                await RoleManager.CreateAsync(new Role { Name = roleName });
            }
            else
            {
                if(await UserManager.IsInRoleAsync(user, roleName))
                {
                    return;
                }

                await UserManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
