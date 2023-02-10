using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Network.Models.KuWo;
public class HttpTagSongMenuResult
{
    public int code { get; set; }
    public long curTime { get; set; }
    public HttpTagSongMenuResultData data { get; set; }
    public string msg { get; set; }
    public string profileId { get; set; }
    public string reqId { get; set; }
    public string tId { get; set; }
}

public class HttpTagSongMenuResultData
{
    public int total { get; set; }
    public List<HttpTagSongMenuResultDataDatum> data { get; set; }
    public int rn { get; set; }
    public int pn { get; set; }
}

public class HttpTagSongMenuResultDataDatum
{
    public string img { get; set; }
    public string uname { get; set; }
    public string lossless_mark { get; set; }
    public string favorcnt { get; set; }
    public string isnew { get; set; }
    public string extend { get; set; }
    public string uid { get; set; }
    public string total { get; set; }
    public string commentcnt { get; set; }
    public string imgscript { get; set; }
    public string digest { get; set; }
    public string name { get; set; }
    public string listencnt { get; set; }
    public string id { get; set; }
    public string attribute { get; set; }
    public string radio_id { get; set; }
    public string desc { get; set; }
    public string info { get; set; }
}
