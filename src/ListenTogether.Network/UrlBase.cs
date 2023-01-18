namespace ListenTogether.Network;
public class UrlBase
{
    public class NetEase
    {
        public const string Index = "https://music.163.com";
        public const string GetHotTagsUrl = "https://music.163.com/discover";
        public const string GetAllTypesUrl = "https://music.163.com/discover/playlist";        
        public const string Suggest = "https://music.163.com/weapi/search/suggest/web?csrf_token=";
        public const string Search = "https://music.163.com/weapi/cloudsearch/get/web?csrf_token=";
        public const string GetMusic = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";
        public const string Lyric = "https://music.163.com/weapi/song/lyric?csrf_token=";
        public const string GetMusicPlayPage = "https://music.163.com/#/song";
    }

    public class KuGou
    {
        public const string Index = "https://www.kugou.com/";
        public const string Search = "https://complexsearch.kugou.com/v2/search/song";
        public const string GetMusic = "https://wwwapi.kugou.com/yy/index.php";
        public const string GetMusicPlayPage = "https://www.kugou.com/song";
    }

    public class MiGu
    {
        public const string Index = "https://music.migu.cn/v3";
        public const string GetTagsUrl = "https://music.migu.cn/v3/music/playlist";
        public const string GetMusicTagPlayUrl = "https://music.migu.cn/v3/music/playlist";
        public const string GetTopMusicsUrl = "https://music.migu.cn/v3/music/top/";
        public const string GetTagMusicsUrl = "https://music.migu.cn/v3/music/playlist/";
        public const string Search = "https://music.migu.cn/v3/search";
        public const string GetMusicDetailUrl = "https://c.musicapp.migu.cn/MIGUM2.0/v1.0/content/resourceinfo.do";
        public const string GetMusicPlayUrl = "https://h5.nf.migu.cn/app/providers/api/v2/song.listen.ask";
        public const string PlayUrlDomain = "http://freetyst.nf.migu.cn";
        public const string GetMusicPlayPage = "https://music.migu.cn/v3/music/song";
    }

    public class KuWo
    {
        public const string HotWord = "http://m.kuwo.cn/newh5app/search";
        public const string Search = "http://www.kuwo.cn/api/www/search/searchMusicBykeyWord";
        public const string GetMusicUrl = "http://antiserver.kuwo.cn/anti.s";
        public const string GetMusicDetail = "http://www.kuwo.cn/newh5/singles/songinfoandlrc";
        public const string GetMusicPlayPage = "https://www.kuwo.cn/play_detail";
    }
}