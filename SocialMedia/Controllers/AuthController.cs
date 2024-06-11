using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Dtos;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Domain.Entites;


namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly ITokenService _tokenService;
        //private readonly IWelcomeService _welcomeService;

        public AuthController(UserManager<Member> userManager, SignInManager<Member> signInManager,
            ITokenService tokenService) //, IWelcomeService welcomeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            //_welcomeService = welcomeService;
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

                //BackgroundJob.Enqueue(() => _welcomeService.WelcomeMessage(registerDto.Username));

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
        public string GetMyToken() // temporary funtion to help me save time without authorizing everytime
        {
            return "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9." +
                "eyJuYW1laWQiOiJiMjI5NWZiZC04ZTk5LTRmMjctOTVlYS03YmYwN2Y3NjdkMzkiLCJlbWFpbCI6Imdpb3JnQGV4YW1wbGUuY29tIiwiZ2l2ZW5fbmFtZSI6Ikdpb3JnaSIsIm5iZiI6MTcxODA4MzE2MywiZXhwIjoxNzE4Njg3OTYzLCJpYXQiOjE3MTgwODMxNjMsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzMTciLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQ0MzE3In0." +
                "NtVREtWKKrQOFxt8fGQf9JktlCDKEIqvgOk4v4A7NcBGYfDptvVLk82QyVd3RSi2Xvm4FoMLLq-KwPoRcYM83g";
        }
    }
}
