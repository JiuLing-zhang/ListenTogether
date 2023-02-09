using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Network.Models.KuWo;

public class HttpMusicTagResult
{
    public int code { get; set; }
    public List<HttpMusicTagDatum> data { get; set; }
}

public class HttpMusicTagDatum
{
    public List<HttpMusicTagDatumData> data { get; set; }
    public string name { get; set; }
}

public class HttpMusicTagDatumData
{
    public string name { get; set; }
    public string id { get; set; }
}
