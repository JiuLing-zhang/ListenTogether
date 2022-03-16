using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Models;

namespace MusicPlayerOnline.Api.Authorization;
public interface IJwtUtils
{
    public string GenerateJwtToken(UserEntity user);
    public int? ValidateJwtToken(string? token);
    public RefreshTokenEntity GenerateRefreshToken(string ipAddress);
}

public class JwtUtils : IJwtUtils
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public JwtUtils(DataContext context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var id = jwtToken.Claims.First(x => x.Type == "Id").Value;

            // return user id from JWT token if validation successful
            return Convert.ToInt32(id);
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

    public RefreshTokenEntity GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshTokenEntity
        {
            Token = GetUniqueToken(),
            // token is valid for 7 days
            ExpireTime = DateTime.Now.AddDays(7),
            CreateTime = DateTime.Now,
            CreateIp = ipAddress
        };

        return refreshToken;
    }
    private string GetUniqueToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        // ensure token is unique by checking against db
        var tokenExist = _context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));
        if (tokenExist)
        {
            return GetUniqueToken();
        }

        return token;
    }
}