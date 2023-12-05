using Capstone.Application.DTOs.Account;
using Capstone.Application.Exceptions;
using Capstone.Application.Interfaces;
using Capstone.Domain.Entities;
using Capstone.Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JWTSettings _token;

        public TokenService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JWTSettings> tokenOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = tokenOptions.Value;
        }

        public async Task<TokenResponse> Authenticate(TokenRequest request, string ipAddress)
        {
            if (await IsValidUser(request.Username, request.Password))
            {
                User user = await GetUserByName(request.Username);

                if (user != null)
                {
                    string jwtToken = await GenerateJwtToken(user);

                    return new TokenResponse(jwtToken);
                }
            }

            throw new ApiException("Error occured");
        }

        private async Task<bool> IsValidUser(string username, string password)
        {
            User user = await GetUserByName(username);

            if (user == null)
            {
                throw new ApiException("Username and password are incorrect");
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (!signInResult.Succeeded)
            {
                throw new ApiException("Username and password are incorrect");
            }
            return signInResult.Succeeded;
        }

        private async Task<User> GetUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();
            byte[] secret = Encoding.ASCII.GetBytes(_token.Key);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("name", $"{user.Name}"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            if (!string.IsNullOrEmpty(user.Email))
            {
                var claimEmail = new Claim(ClaimTypes.Email, user.Email);
                claims.Add(claimEmail);
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_token.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
