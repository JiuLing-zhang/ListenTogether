namespace ListenTogether.Network.Models.KuWo;
public class HttpTagSongMenuResult
{
    public int code { get; set; }
    public HttpTagSongMenuResultData? data { get; set; }
}

public class HttpTagSongMenuResultData
{
    public List<HttpTagSongMenuResultDataDatum>? data { get; set; }
}

public class HttpTagSongMenuResultDataDatum
{
    public string img { get; set; } = null!;
    public string name { get; set; } = null!;
    public string id { get; set; } = null!;
}
