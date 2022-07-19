namespace ListenTogether.Network;
public class UrlBase
{
    public class NetEase
    {
        public const string Suggest = "https://music.163.com/weapi/search/suggest/web?csrf_token=";
        public const string Search = "https://music.163.com/weapi/cloudsearch/get/web?csrf_token=";
        public const string GetMusic = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";
        public const string Lyric = "https://music.163.com/weapi/song/lyric?csrf_token=";
    }

    public class KuGou
    {
        public const string Search = "https://complexsearch.kugou.com/v2/search/song";
        public const string GetMusic = "https://wwwapi.kugou.com/yy/index.php";
    }

    public class MiGu
    {
        public const string Search = "https://www.migu.cn/search.html";
        public const string GetMusicDetailUrl = "https://c.musicapp.migu.cn/MIGUM2.0/v1.0/content/resourceinfo.do";
        public const string GetMusicPlayUrl = "https://h5.nf.migu.cn/app/providers/api/v2/song.listen.ask";
    }

    public class KuWo
    {
        public const string HotWord = "http://m.kuwo.cn/newh5app/search";
        public const string Search = "http://www.kuwo.cn/api/www/search/searchMusicBykeyWord";
        public const string GetMusicUrl = "http://antiserver.kuwo.cn/anti.s";
        public const string GetMusicDetail = "http://www.kuwo.cn/newh5/singles/songinfoandlrc";
    }
}