namespace ListenTogether.Network.Models.KuWo;
public class HttpSongMenuResult
{
    public int code { get; set; }
    public HttpSongMenuData? data { get; set; }
}

public class HttpSongMenuData
{
    public List<HttpSongMenuDataList>? musicList { get; set; }
}

public class HttpSongMenuDataList
{
    public string artist { get; set; } = null!;
    public string pic { get; set; } = null!;
    public int rid { get; set; }
    public int duration { get; set; }
    public string album { get; set; } = null!;
    public string name { get; set; } = null!;
    public HttpSongMenuDataListPayInfo payInfo { get; set; } = null!;
}

public class HttpSongMenuDataListPayInfo
{
    public string listen_fragment { get; set; } = null!;
}

