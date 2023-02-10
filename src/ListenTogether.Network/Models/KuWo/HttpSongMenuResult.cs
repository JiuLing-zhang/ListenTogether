namespace ListenTogether.Network.Models.KuWo;
public class HttpSongMenuResult
{
    public int code { get; set; }
    public HttpSongMenuData data { get; set; }
}

public class HttpSongMenuData
{
    public List<HttpSongMenuDataList> musicList { get; set; }
}

public class HttpSongMenuDataList
{
    public string artist { get; set; }
    public string pic { get; set; }
    public int rid { get; set; }
    public int duration { get; set; }
    public string album { get; set; }
    public string name { get; set; }
    public HttpSongMenuDataListPayInfo payInfo { get; set; }
}

public class HttpSongMenuDataListPayInfo
{
    public string listen_fragment { get; set; }
}

