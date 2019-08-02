using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Template.Core.Exceptions;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Core.Settings;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> userManager;
        private readonly TokenSettings tokenSettings;
        private readonly ISharedResources localizer;

        public TokenService(
            UserManager<User> userManager,
            IOptions<TokenSettings> tokenSettings,
            ISharedResources localizer)
        {
            this.userManager = userManager;
            this.tokenSettings = tokenSettings.Value;
            this.localizer = localizer;
            this.ThrowIfInvalidSettings(this.tokenSettings);
        }

        public async Task<TokenDto> AuthenticateAsync(CredentialsDto credentials)
        {
            var user = await this.userManager.FindByNameAsync(credentials.UserName);
            if (user == null)
            {
                var message = this.localizer.GetAndApplyKeys("NotFound", "User");
                throw new NotFoundException(message);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, credentials.Password);
            if (!passwordIsValid)
            {
                throw new InvalidPasswordException(this.localizer.Get("InvalidPassword"));
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var encodedToken = this.GenerateEncodedToken(user, roles);
            var tokenResult = this.GenerateTokeResult(encodedToken, user, roles);
            return tokenResult;
        }

        private string GenerateEncodedToken(User user, IEnumerable<string> roles)
        {
            var issuedAt = this.ToUnixEpochDate(this.tokenSettings.IssuedAt);

            var claims = new[]
             {
                 new Claim(Constants.ClaimTypes.Id, user.Id.ToString(), ClaimValueTypes.Integer32),
                 new Claim(Constants.ClaimTypes.Role, roles.Join()),
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, this.tokenSettings.JtiGenerator),
                 new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString(), ClaimValueTypes.Integer64)
             };

            var token = new JwtSecurityToken(
                issuer: this.tokenSettings.Issuer,
                audience: this.tokenSettings.Audience,
                claims: claims,
                notBefore: this.tokenSettings.NotBefore,
                expires: this.tokenSettings.Expiration,
                signingCredentials: this.tokenSettings.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private TokenDto GenerateTokeResult(string encodedToken, User user, IEnumerable<string> roles)
        {
            var tokenResult = new TokenDto
            {
                Token = encodedToken,
                Id = user.Id,
                Name = user.FullName,
                UserName = user.UserName,
                Role = roles.Join(),
                IssuedAt = this.tokenSettings.IssuedAt,
                Expires = this.tokenSettings.Expiration
            };
            return tokenResult;
        }

        private long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        private void ThrowIfInvalidSettings(TokenSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException(nameof(TokenSettings.ValidFor));
            }

            if (settings.SigningCredentials == null)
            {
                throw new ArgumentException(nameof(TokenSettings.SigningCredentials));
            }

            if (settings.JtiGenerator == null)
            {
                throw new ArgumentException(nameof(TokenSettings.JtiGenerator));
            }
        }
    }
}
