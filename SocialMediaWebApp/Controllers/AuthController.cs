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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("404", "Uh oh");
                return BadRequest(ModelState);
            }

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
                "eyJuYW1laWQiOiIzMmNiZmE2OS1iNzNjLTQ3ZDUtYTE0OS0yNmQ0ZjZhMTk2NDMiLCJlbWFpbCI6Imdpb3JnaUBleGFtcGxlLmNvbSIsImdpdmVuX25hbWUiOiJHaW9yZ2kiLCJuYmYiOjE3MTA5MzM1NTIsImV4cCI6MTcxMTUzODM1MiwiaWF0IjoxNzEwOTMzNTUyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjQ0MzI3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0NDMyNy8ifQ." +
                "Bcb3VaWIBgytxRheIYUWyll8IaKEaAhJ1WuqEDQecVXMWw3GZuLzoiQn4St84UUwUXCplun2r8lLX54SRPSrXQ";
        }
    }
}
