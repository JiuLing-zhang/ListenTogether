namespace ListenTogether.Network.Models.KuWo;

public class HttpMusicTagResult
{
    public int code { get; set; }
    public List<HttpMusicTagDatum>? data { get; set; }
}

public class HttpMusicTagDatum
{
    public List<HttpMusicTagDatumData> data { get; set; } = null!;
    public string name { get; set; } = null!;
}

public class HttpMusicTagDatumData
{
    public string name { get; set; } = null!;
    public string id { get; set; } = null!;
}
