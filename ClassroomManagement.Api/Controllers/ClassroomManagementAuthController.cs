using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagement.Api.Controllers
{
    public class ClassroomManagementAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        public ClassroomManagementAuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto newUser)
        {
            var identityUser = new IdentityUser()
            {
                UserName = newUser.Username,
                Email = newUser.Email,
            };
            var identityResult = await userManager.CreateAsync(identityUser, newUser.Password);
            if (identityResult.Succeeded)
            {
                if (newUser.Roles?.Any() is true)
                {
                    string[] roles = { "Reader", "Writer" };
                    foreach (var role in newUser.Roles)
                    {
                        if (roles.Contains(role) is false)
                        {
                            return Ok("User was registered but roles couldnt be added.");
                        }
                    }
                    identityResult = await userManager.AddToRolesAsync(identityUser, newUser.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered with the given roles now you can login.");
                    }
                    else
                    {
                        return Ok("User was registered but roles couldn't be added now u can login.");
                    }
                }
            }
            return BadRequest("User couldn't be registered.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user is not null)
            {
                var passwordResult = await userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (passwordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles?.Any() is true)
                    {
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                        return Ok(new { JwtToken = jwtToken });
                    }
                }
            }
            return BadRequest("Email or Password is incorrect.");
        }
    }
}