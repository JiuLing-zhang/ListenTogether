using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.ErrorHandler;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Services;
internal class UserService : IUserService
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

    public async Task<Result> Register(User dto)
    {
        var isUserExist = await _context.Users.AnyAsync(x => x.Username == dto.Username);
        if (isUserExist)
        {
            return new Result(1, "用户名已存在");
        }

        string salt = JiuLing.CommonLibs.GuidUtils.GetFormatN();
        string password = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower($"{dto.Password}{salt}");

        var user = new UserEntity()
        {
            Username = dto.Username,
            Password = password,
            Salt = salt,
            IsEnable = false,
            Nickname = dto.Username,
            Avatar = "",
            CreateTime = DateTime.Now,
        };

        _context.Users.Add(user);
        var count = await _context.SaveChangesAsync();
        if (count == 0)
        {
            return new Result(2, "注册失败");
        }
        return new Result(0, "注册成功，请等待管理员审核");
    }

    public async Task<Result<UserDto>> Login(User dto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == dto.Username && x.IsEnable);
        if (user == null)
        {
            return new Result<UserDto>(1, "用户名或密码不正确", null);
        }

        string password = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower($"{dto.Password}{user.Salt}");
        if (password != user.Password)
        {
            return new Result<UserDto>(1, "用户名或密码不正确", null);
        }

        var token = _jwtUtils.GenerateToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken();
        user.RefreshTokens.Clear();
        user.RefreshTokens.Add(refreshToken);

        _context.Update(user);
        await _context.SaveChangesAsync();

        return new Result<UserDto>(0, "", new UserDto()
        {
            UserName = user.Username,
            Nickname = user.Nickname,
            Avatar = user.Avatar,
            Token = token,
            RefreshToken = refreshToken.Token
        });
    }

    public async Task<Result<UserDto>> RefreshToken(string token)
    {
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(r => r.Token == token));
        if (user == null)
        {
            throw new AppException("无效Token");
        }

        var refreshToken = user.RefreshTokens.Single();
        if (refreshToken.IsExpired)
        {
            throw new AppException("Token已过期");
        }
        user.RefreshTokens.Clear();

        var newToken = _jwtUtils.GenerateToken(user);
        var newRefreshToken = _jwtUtils.GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);

        _context.Update(user);
        await _context.SaveChangesAsync();

        return new Result<UserDto>(0, "更新成功", new UserDto()
        {
            UserName = user.Username,
            Nickname = user.Nickname,
            Avatar = user.Avatar,
            Token = newToken,
            RefreshToken = newRefreshToken.Token
        });
    }

    public async Task Logout(int id)
    {
        var user = _context.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
        {
            throw new AppException("用户不存在");
        }

        user.RefreshTokens.Clear();
        await _context.SaveChangesAsync();
    }

    public async Task<Result<UserDto>> GetUserInfo(int id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id && x.IsEnable);
        if (user == null)
        {
            return new Result<UserDto>(1, "用户不存在", null);
        }

        return new Result<UserDto>(0, "", new UserDto()
        {
            UserName = user.Username,
            Nickname = user.Nickname,
            Avatar = user.Avatar
        });
    }

    public async Task<UserEntity?> GetOneEnableAsync(int id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id && x.IsEnable);
        if (user == null)
        {
            return null;
        }

        return user;
    }
}