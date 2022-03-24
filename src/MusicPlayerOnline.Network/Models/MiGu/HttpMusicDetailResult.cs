namespace MusicPlayerOnline.Network.Models.MiGu
{
    public class HttpMusicDetailResult
    {
        public string code { get; set; }
        public string info { get; set; }
        public Resource[] resource { get; set; }
    }

    public class Resource
    {
        public string resourceType { get; set; }
        public string refId { get; set; }
        public string copyrightId { get; set; }
        public string contentId { get; set; }
        public string songId { get; set; }
        public string songName { get; set; }
        public string singerId { get; set; }
        public string singer { get; set; }
        public string albumId { get; set; }
        public string album { get; set; }
        public List<Albumimg> albumImgs { get; set; }
        public Opnumitem opNumItem { get; set; }
        public string toneControl { get; set; }
        public Relatedsong[] relatedSongs { get; set; }
        public Rateformat[] rateFormats { get; set; }
        public Newrateformat[] newRateFormats { get; set; }
        public Z3dcode z3dCode { get; set; }
        public string lrcUrl { get; set; }
        public string digitalColumnId { get; set; }
        public string copyright { get; set; }
        public bool validStatus { get; set; }
        public string songDescs { get; set; }
        public string songAliasName { get; set; }
        public string isInDAlbum { get; set; }
        public string isInSideDalbum { get; set; }
        public string isInSalesPeriod { get; set; }
        public string songType { get; set; }
        public string mrcUrl { get; set; }
        public string invalidateDate { get; set; }
        public string dalbumId { get; set; }
        public string trcUrl { get; set; }
        public string vipType { get; set; }
        public string scopeOfcopyright { get; set; }
        public string auditionsType { get; set; }
        public string firstIcon { get; set; }
        public string translateName { get; set; }
        public string chargeAuditions { get; set; }
        public string oldChargeAuditions { get; set; }
        public string songIcon { get; set; }
        public Coderate codeRate { get; set; }
        public string isDownload { get; set; }
        public string hasMv { get; set; }
        public string topQuality { get; set; }
        public string preSale { get; set; }
        public string isShare { get; set; }
        public string isCollection { get; set; }
        public string length { get; set; }
        public Singerimg singerImg { get; set; }
        public string songNamePinyin { get; set; }
        public string albumNamePinyin { get; set; }
        public Artist[] artists { get; set; }
        public string landscapImg { get; set; }
        public string vipLogo { get; set; }
        public string vipDownload { get; set; }
        public string firstPublish { get; set; }
        public string[] showTag { get; set; }
        public bool materialValidStatus { get; set; }
    }

    public class Opnumitem
    {
        public int playNum { get; set; }
        public string playNumDesc { get; set; }
        public int keepNum { get; set; }
        public string keepNumDesc { get; set; }
        public int commentNum { get; set; }
        public string commentNumDesc { get; set; }
        public int shareNum { get; set; }
        public string shareNumDesc { get; set; }
        public int orderNumByWeek { get; set; }
        public string orderNumByWeekDesc { get; set; }
        public int orderNumByTotal { get; set; }
        public string orderNumByTotalDesc { get; set; }
        public int thumbNum { get; set; }
        public string thumbNumDesc { get; set; }
        public int followNum { get; set; }
        public string followNumDesc { get; set; }
        public int subscribeNum { get; set; }
        public string subscribeNumDesc { get; set; }
        public int livePlayNum { get; set; }
        public string livePlayNumDesc { get; set; }
        public int popularNum { get; set; }
        public string popularNumDesc { get; set; }
        public int bookingNum { get; set; }
        public string bookingNumDesc { get; set; }
    }

    public class Z3dcode
    {
        public string resourceType { get; set; }
        public string formatType { get; set; }
        public string price { get; set; }
        public string iosUrl { get; set; }
        public string androidUrl { get; set; }
        public string androidFileType { get; set; }
        public string iosFileType { get; set; }
        public string iosSize { get; set; }
        public string androidSize { get; set; }
        public string iosFormat { get; set; }
        public string androidFormat { get; set; }
        public string androidFileKey { get; set; }
        public string iosFileKey { get; set; }
        public string h5Url { get; set; }
        public string h5Size { get; set; }
        public string h5Format { get; set; }
    }

    public class Coderate
    {
        public PQ PQ { get; set; }
        public HQ HQ { get; set; }
        public SQ SQ { get; set; }
        public ZQ ZQ { get; set; }
        public Z3D Z3D { get; set; }
    }

    public class PQ
    {
        public string codeRateChargeAuditions { get; set; }
        public string isCodeRateDownload { get; set; }
        public string codeRateFileSize { get; set; }
    }

    public class HQ
    {
        public string codeRateChargeAuditions { get; set; }
        public string isCodeRateDownload { get; set; }
    }

    public class SQ
    {
        public string codeRateChargeAuditions { get; set; }
        public string isCodeRateDownload { get; set; }
        public string contentIdSQ { get; set; }
    }

    public class ZQ
    {
        public string codeRateChargeAuditions { get; set; }
        public string isCodeRateDownload { get; set; }
    }

    public class Z3D
    {
        public string codeRateChargeAuditions { get; set; }
        public string isCodeRateDownload { get; set; }
    }

    public class Singerimg
    {
        public _112 _112 { get; set; }
    }

    public class _112
    {
        public string singerName { get; set; }
        public Miguimgitem[] miguImgItems { get; set; }
    }

    public class Miguimgitem
    {
        public string imgSizeType { get; set; }
        public string img { get; set; }
        public string fileId { get; set; }
    }

    public class Albumimg
    {
        public string imgSizeType { get; set; }
        public string img { get; set; }
    }

    public class Relatedsong
    {
        public string resourceType { get; set; }
        public string resourceTypeName { get; set; }
        public string copyrightId { get; set; }
        public string productId { get; set; }
    }

    public class Rateformat
    {
        public string resourceType { get; set; }
        public string formatType { get; set; }
        public string url { get; set; }
        public string format { get; set; }
        public string size { get; set; }
        public string fileType { get; set; }
        public string price { get; set; }
        public string iosUrl { get; set; }
        public string androidUrl { get; set; }
        public string androidFileType { get; set; }
        public string iosFileType { get; set; }
        public string iosSize { get; set; }
        public string androidSize { get; set; }
        public string iosFormat { get; set; }
        public string androidFormat { get; set; }
        public string iosAccuracyLevel { get; set; }
        public string androidAccuracyLevel { get; set; }
    }

    public class Newrateformat
    {
        public string resourceType { get; set; }
        public string formatType { get; set; }
        public string url { get; set; }
        public string format { get; set; }
        public string size { get; set; }
        public string fileType { get; set; }
        public string price { get; set; }
        public string iosUrl { get; set; }
        public string androidUrl { get; set; }
        public string androidFileType { get; set; }
        public string iosFileType { get; set; }
        public string iosSize { get; set; }
        public string androidSize { get; set; }
        public string iosFormat { get; set; }
        public string androidFormat { get; set; }
        public string iosAccuracyLevel { get; set; }
        public string androidAccuracyLevel { get; set; }
        public string androidNewFormat { get; set; }
        public int iosBit { get; set; }
        public int androidBit { get; set; }
    }

    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nameSpelling { get; set; }
    }

}
