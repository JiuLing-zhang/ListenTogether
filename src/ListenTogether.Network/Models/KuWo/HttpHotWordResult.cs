using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Network.Models.KuWo;
public class HttpHotWordResult
{
    public int code { get; set; }
    public List<string>? data { get; set; }
}
