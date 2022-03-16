using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.ErrorHandler;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Request;
using MusicPlayerOnline.Model.Response;

namespace MusicPlayerOnline.Api.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        public UserService(DataContext context, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }
        public async Task<UserDto> Login(User dto, string ipAddress)
        {
            var user = await GetOneAsync(dto.Username);
            if (user == null)
            {
                throw new AppException("用户名或密码不正确");
            }

            string password = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower($"{dto.Password}{user.Salt}");
            if (password != user.Password)
            {
                throw new AppException("用户名或密码不正确");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _context.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto()
            {
                UserName = user.Username,
                Nickname = user.Nickname,
                Avatar = user.Avatar,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        private void RemoveOldRefreshTokens(UserEntity user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.CreateTime.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        public async Task<UserEntity?> GetOneAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == userName && x.IsEnable);
        }
    }
}
