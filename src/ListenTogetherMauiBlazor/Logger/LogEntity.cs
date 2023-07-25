using SQLite;

namespace ListenTogetherMauiBlazor.Logger;

[Table("LogDetail")]
internal class LogEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int LogType { get; set; }
    public string Message { get; set; } = null!;
    public long CreateTime { get; set; }
}