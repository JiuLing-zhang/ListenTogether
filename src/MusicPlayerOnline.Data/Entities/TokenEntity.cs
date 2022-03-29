using SQLite;

namespace MusicPlayerOnline.Data.Entities;

[Table("TokenEntity")]
internal class TokenEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Token { get; set; } = "";
    public string RefreshToken { get; set; } = "";
}