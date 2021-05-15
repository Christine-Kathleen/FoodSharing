using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Authentication;   
using Microsoft.AspNetCore.Identity;    
using Microsoft.Extensions.Configuration;  
using Microsoft.IdentityModel.Tokens;   
using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Text;
using Microsoft.AspNetCore.Authorization;
using FoodSharing.Models;
using FoodSharing.Services;
using FoodSharing;
using static FoodSharing.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GetUser")]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> GetUser([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                user.ConcurrencyStamp = "";
                user.PasswordHash = "";
                user.SecurityStamp = "";
                return user;
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo    
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userNameExists = await userManager.FindByNameAsync(model.UserName);
            var userEmailExists = await userManager.FindByEmailAsync(model.Email);
            if (userNameExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status= Status.Error, Message = APIMessages.ErrorRegisterName });
            }
            if (userEmailExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response {  Status = Status.Error, Message = APIMessages.ErrorRegisterEmail });
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Telephone
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnRegisterFailed });

            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }
        [HttpPatch]
        [Route("UpdateUserPassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            else if(await userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnPasswordChange });

                return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnPasswordCheck });
        }
        [HttpPatch]
        [Route("UpdateUserProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            user.Description = model.Description;
            var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnUpdate });

            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }

        [HttpPost]
        [Route("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnDeletion });

            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }
    }
}