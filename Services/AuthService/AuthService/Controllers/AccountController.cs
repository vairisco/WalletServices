using AuthServer.Infrastructure.Data.Identity;
using IdentityModel;
using AuthService.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.ResponseModels;
using AuthService.Repositories.Interfaces;
using AuthService.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using AuthService.Helpers;

namespace AuthService.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Repositories.Interfaces.ITokenService _tokenService;
        private readonly Repositories.Interfaces.IUserRepository _userRepository;

        private string generatedToken = null;
        public AccountController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            Repositories.Interfaces.ITokenService tokenService,
            IUserRepository userRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = config;
            _userRepository = userRepository;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<RegisterResModel> Register(RegisterViewModel model)
        {
            var user = new User
            {
                Name = model.Email,
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            } else
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            result = await _userManager.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Email, model.Email)
                        });

            if (!result.Succeeded)
            {
                return new RegisterResModel(false, result.Errors.First().Description);
            }
            return new RegisterResModel(true, "Đăng ký tài khoản thành công");

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<UserDTO>> Login(LoginViewModel loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO
            {
                Username = user.UserName,
                Token = await TokenHelper.BuildToken(user, _userManager, _roleManager, _configuration),
            };
        }
    }
}
