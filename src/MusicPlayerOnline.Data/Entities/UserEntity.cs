using SQLite;

namespace MusicPlayerOnline.Data.Entities;
//TODO 文件重命名

[Table("UserBase")]
internal class UserEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Avatar { get; set; } = "";
    public string Token { get; set; } = "";
    public string RefreshToken { get; set; } = "";
}