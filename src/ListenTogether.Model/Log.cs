namespace MusicPlayerOnline.Model;

public class Log
{
    public Int64 Timestamp { get; set; }
    public int LogType { get; set; }
    public string Message { get; set; } = null!;
}