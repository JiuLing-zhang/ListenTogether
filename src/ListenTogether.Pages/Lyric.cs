namespace ListenTogether.Pages;
internal class Lyric
{
    public long PositionMillisecond { get; set; }
    public string Info { get; set; }
    public bool IsHighlight { get; set; }

    public Lyric(long positionMillisecond, string info)
    {
        PositionMillisecond = positionMillisecond;
        Info = info;
        IsHighlight = false;
    }
}