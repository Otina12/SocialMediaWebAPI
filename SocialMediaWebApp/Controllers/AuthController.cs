using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<Member> userManager, SignInManager<Member> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] MemberRegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("404", "Uh oh");
                    return BadRequest(ModelState);
                }

                var newMember = new Member
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var created = await _userManager.CreateAsync(newMember, registerDto.Password);

                if(!created.Succeeded)
                {
                    return StatusCode(500, created.Errors);
                }

                var result = await _userManager.AddToRoleAsync(newMember, "User");
                if (!result.Succeeded)
                {
                    return StatusCode(500, result.Errors);
                }

                var newUserDto = new NewMemberDto
                {
                    Username = registerDto.Username,
                    Email = newMember.Email,
                    Token = _tokenService.CreateToken(newMember)
                };
                return Ok(newUserDto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] MemberLoginDto loginDto)
        {
            var member = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

            if(member is null)
            {
                return Unauthorized("Wrong username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(member, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Password is incorrect");
            }

            var newMemberDto = new NewMemberDto
            {
                Username = member.UserName!,
                Email = member.Email!,
                Token = _tokenService.CreateToken(member)
            };

            return Ok(newMemberDto);
        }

        [HttpGet("GetGiorgiToken")]
        public string GetMyToken() // temporary funtion to help me save time with testing authorization
        {
            return "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9." +
                "eyJuYW1laWQiOiJkMGNiODNmMS05MTMyLTRmYTYtOWU3Yy1hMDZkOTAzNzY4Y2IiLCJlbWFpbCI6Imdpb3JndW5hQGV4YW1wbGUuY29tIiwiZ2l2ZW5fbmFtZSI6Imdpb3JndW5hIiwibmJmIjoxNzExODgyODMwLCJleHAiOjE3MTI0ODc2MzAsImlhdCI6MTcxMTg4MjgzMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDMyNyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzMjcvIn0." +
                "FZvgjR7dJGj2h2jwguiUfHxDu8EkPwRLS47jZY42Xe_y56SkLVh8GcM1N8qQLsJoBI8q9C6A_4NCv5-9fdZm0g";
        }
    }
}
