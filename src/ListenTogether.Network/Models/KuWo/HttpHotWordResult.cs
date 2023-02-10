using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Network.Models.KuWo;
public class HttpHotWordResult
{
    public int code { get; set; }
    public long curTime { get; set; }
    public List<string> data { get; set; }
    public string msg { get; set; }
    public string profileId { get; set; }
    public string reqId { get; set; }
    public string tId { get; set; }
}
