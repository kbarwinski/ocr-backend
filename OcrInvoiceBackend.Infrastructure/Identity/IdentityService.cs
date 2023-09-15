using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Infrastructure.Identity
{
    public class UserInfo : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }

    public class IdentityService : IIdentityService
    {
        private readonly Dictionary<string, List<string>> roleHierarchy = new Dictionary<string, List<string>>
        {
            { "Admin", new List<string> { "User" } }
        };

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityService(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<OperationResult> CreateUserAsync(string email, string password, string roleName)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                return await AssignRoleAsync(user, roleName);
            }

            return OperationResult.Failure;
        }

        private async Task<OperationResult> AssignRoleAsync(ApplicationUser user, string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);

                if (roleHierarchy.ContainsKey(role))
                {
                    foreach (var impliedRole in roleHierarchy[role])
                    {
                        await _userManager.AddToRoleAsync(user, impliedRole);
                    }
                }

                return OperationResult.Success;
            }
            else
                return OperationResult.Failure;
        }

        public async Task<IUser?> ValidateJwtTokenAsync(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new UserInfo
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = (List<string>)roles
                };
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public async Task<string?> GenerateJwtTokenAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, email),
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
            }
            return null;
        }

        public async Task<OperationResult> SignInAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded) return OperationResult.Success;
            if (result.IsLockedOut) return OperationResult.UserLockedOut;
            if (result.IsNotAllowed) return OperationResult.EmailNotConfirmed;
            if (result.RequiresTwoFactor) return OperationResult.Failure;
            return OperationResult.InvalidCredentials;
        }

        public async Task<OperationResult> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            return result.Succeeded ? OperationResult.Success : OperationResult.Failure;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
