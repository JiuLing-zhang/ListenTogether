namespace ListenTogether.Model;

public class MusicPosition
{
    /// <summary>
    /// 当前进度（毫秒）
    /// </summary>
    public TimeSpan position { get; set; }
    /// <summary>
    /// 总进度（毫秒）
    /// </summary>
    public TimeSpan Duration { get; set; }
    public double PlayProgress { get; set; }
}