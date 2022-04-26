using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Data.Repositories.Local;

public class UserLocalRepository : IUserLocalRepository
{
    public bool Write(User user)
    {
        var dbUser = DatabaseProvide.Database.Table<UserEntity>().FirstOrDefault();
        int count;
        if (dbUser != null)
        {
            dbUser.Username = user.Username;
            dbUser.Nickname = user.Nickname;
            dbUser.Avatar = user.Avatar;
            dbUser.Token = user.Token;
            dbUser.RefreshToken = user.RefreshToken;
            count = DatabaseProvide.Database.Update(dbUser);
        }
        else
        {
            var myUser = new UserEntity()
            {
                Username = user.Username,
                Nickname = user.Nickname,
                Avatar = user.Avatar,
                Token = user.Token,
                RefreshToken = user.RefreshToken
            };
            count = DatabaseProvide.Database.Insert(myUser);
        }

        return count != 0;
    }

    public User? Read()
    {
        var tokens = DatabaseProvide.Database.Table<UserEntity>().ToList();

        if (tokens == null || tokens.Count != 1)
        {
            return default;
        }

        return new User()
        {
            Username = tokens[0].Username,
            Nickname = tokens[0].Nickname,
            Avatar = tokens[0].Avatar,
            Token = tokens[0].Token,
            RefreshToken = tokens[0].RefreshToken
        };
    }

    public void Remove()
    {
        DatabaseProvide.Database.DeleteAll<UserEntity>();
    }

    public void UpdateToken(TokenInfo tokenInfo)
    {
        var dbUser = DatabaseProvide.Database.Table<UserEntity>().FirstOrDefault();

        if (dbUser == null)
        {
            return;
        }
        dbUser.Token = tokenInfo.Token;
        dbUser.RefreshToken = tokenInfo.RefreshToken;
        DatabaseProvide.Database.Update(dbUser);
    }
}