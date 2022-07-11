using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Network.Models.KuWo
{
    internal class HttpMusicSearchResult
    {
        public Rcm[] rcm { get; set; }
        public MusicData[] music { get; set; }
    }

    internal class Rcm
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_alt { get; set; }
        public string pic { get; set; }
        public string count_album { get; set; }
        public string count_music { get; set; }
        public string count_play { get; set; }
        public string artist { get; set; }
        public string publish { get; set; }
        public string type { get; set; }
        public string digest { get; set; }
        public string tag { get; set; }
        public string isstar { get; set; }
    }

    internal class MusicData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string album_name { get; set; }
        public string artist_name { get; set; }
        public int mv_status { get; set; }
        public Mv_Pay_Info mv_pay_info { get; set; }
        public int isstar { get; set; }
        public string pay { get; set; }
        public int online { get; set; }
        public Pay_Info pay_info { get; set; }
    }

    internal class Mv_Pay_Info
    {
        public string play { get; set; }
        public string vid { get; set; }
        public string download { get; set; }
    }

    internal class Pay_Info
    {
        public string nplay { get; set; }
        public string play { get; set; }
        public string overseas_nplay { get; set; }
        public string local_encrypt { get; set; }
        public string limitfree { get; set; }
        public string refrain_start { get; set; }
        public Feetype feeType { get; set; }
        public string ndown { get; set; }
        public string download { get; set; }
        public string cannotDownload { get; set; }
        public string overseas_ndown { get; set; }
        public string cannotOnlinePlay { get; set; }
        public string listen_fragment { get; set; }
        public string refrain_end { get; set; }
        public string tips_intercept { get; set; }
    }

    internal class Feetype
    {
        public string song { get; set; }
        public string album { get; set; }
        public string vip { get; set; }
        public string bookvip { get; set; }
    }

}
