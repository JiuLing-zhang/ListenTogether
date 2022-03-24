namespace MusicPlayerOnline.Network.Models.KuGou
{
    public class HttpMusicSearchResult
    {
        public Aggregation[] aggregation { get; set; }
        public int searchfull { get; set; }
        public int total { get; set; }
        public int istagresult { get; set; }
        public int isshareresult { get; set; }
        public int page { get; set; }
        public int chinesecount { get; set; }
        public List[] lists { get; set; }
        public int correctiontype { get; set; }
        public int allowerr { get; set; }
        public string correctionsubject { get; set; }
        public int subjecttype { get; set; }
        public int pagesize { get; set; }
        public int istag { get; set; }
        public string correctiontip { get; set; }
        public int correctionforce { get; set; }
        public Sectag_Info sectag_info { get; set; }
    }

    public class Sectag_Info
    {
        public int is_sectag { get; set; }
    }

    public class Aggregation
    {
        public string key { get; set; }
        public int count { get; set; }
    }

    public class List
    {
        public string Suffix { get; set; }
        public string SongName { get; set; }
        public int OwnerCount { get; set; }
        public int MvType { get; set; }
        public string TopicRemark { get; set; }
        public int SQFailProcess { get; set; }
        public string Source { get; set; }
        public int Bitrate { get; set; }
        public string HQExtName { get; set; }
        public int SQFileSize { get; set; }
        public int Accompany { get; set; }
        public int AudioCdn { get; set; }
        public int MvTrac { get; set; }
        public int SQDuration { get; set; }
        public int recommend_type { get; set; }
        public string PlayCnt { get; set; }
        public string ExtName { get; set; }
        public string Auxiliary { get; set; }
        public int SQPkgPrice { get; set; }
        public int Category { get; set; }
        public string RankInfo { get; set; }
        public int Scid { get; set; }
        public string OriSongName { get; set; }
        public string Uploader { get; set; }
        public int SQBitrate { get; set; }
        public int HQBitrate { get; set; }
        public int Audioid { get; set; }
        public int HiFiQuality { get; set; }
        public Grp[] Grp { get; set; }
        public string OriOtherName { get; set; }
        public int AlbumPrivilege { get; set; }
        public string TopicUrl { get; set; }
        public string SuperFileHash { get; set; }
        public int ASQPrivilege { get; set; }
        public int OldCpy { get; set; }
        public int IsOriginal { get; set; }
        public int Privilege { get; set; }
        public string TagContent { get; set; }
        public int ResBitrate { get; set; }
        public string FileHash { get; set; }
        public Trans_Param trans_param { get; set; }
        public int SQPayType { get; set; }
        public int FoldType { get; set; }
        public int HQPrice { get; set; }
        public int A320Privilege { get; set; }
        public string Type { get; set; }
        public string AlbumID { get; set; }
        public string AlbumName { get; set; }
        public string SQExtName { get; set; }
        public string vvid { get; set; }
        public string FileName { get; set; }
        public string MixSongID { get; set; }
        public string ID { get; set; }
        public int SuperFileSize { get; set; }
        public int SQPrivilege { get; set; }
        public string SQFileHash { get; set; }
        public string SuperExtName { get; set; }
        public int HQPrivilege { get; set; }
        public int SuperBitrate { get; set; }
        public int SuperDuration { get; set; }
        public int HQPkgPrice { get; set; }
        public string ResFileHash { get; set; }
        public int FileSize { get; set; }
        public int ResFileSize { get; set; }
        public string HQFileHash { get; set; }
        public string SongLabel { get; set; }
        public string PublishTime { get; set; }
        public int Publish { get; set; }
        public int mvTotal { get; set; }
        public string MvHash { get; set; }
        public int PkgPrice { get; set; }
        public int M4aSize { get; set; }
        public int Duration { get; set; }
        public string OtherName { get; set; }
        public int PublishAge { get; set; }
        public int SQPrice { get; set; }
        public int ResDuration { get; set; }
        public int[] SingerId { get; set; }
        public int Price { get; set; }
        public int FailProcess { get; set; }
        public string SingerName { get; set; }
        public int HQFailProcess { get; set; }
        public int HQFileSize { get; set; }
        public int HQPayType { get; set; }
        public int HQDuration { get; set; }
        public int PayType { get; set; }
        public int HasAlbum { get; set; }
        public int QualityLevel { get; set; }
        public int SourceID { get; set; }
        public Res Res { get; set; }
    }

    public class Trans_Param
    {
        public int cid { get; set; }
        public int pay_block_tpl { get; set; }
        public int musicpack_advance { get; set; }
        public int display_rate { get; set; }
        public Classmap classmap { get; set; }
        public int display { get; set; }
        public int cpy_attr0 { get; set; }
        public int cpy_grade { get; set; }
        public int cpy_level { get; set; }
        public Hash_Offset hash_offset { get; set; }
        public string appid_block { get; set; }
    }

    public class Classmap
    {
        public int attr0 { get; set; }
    }

    public class Hash_Offset
    {
        public int file_type { get; set; }
        public int start_byte { get; set; }
        public int start_ms { get; set; }
        public string offset_hash { get; set; }
        public int end_ms { get; set; }
        public int end_byte { get; set; }
    }

    public class Res
    {
        public int Privilege { get; set; }
        public int PayType { get; set; }
        public int PkgPrice { get; set; }
        public int Price { get; set; }
        public int FailProcess { get; set; }
    }

    public class Grp
    {
        public string Suffix { get; set; }
        public string SongName { get; set; }
        public int OwnerCount { get; set; }
        public int MvType { get; set; }
        public string TopicRemark { get; set; }
        public int SQFailProcess { get; set; }
        public string Source { get; set; }
        public int Bitrate { get; set; }
        public string HQExtName { get; set; }
        public int SQFileSize { get; set; }
        public int Accompany { get; set; }
        public int AudioCdn { get; set; }
        public int MvTrac { get; set; }
        public int SQDuration { get; set; }
        public int recommend_type { get; set; }
        public string PlayCnt { get; set; }
        public string ExtName { get; set; }
        public string Auxiliary { get; set; }
        public int SQPkgPrice { get; set; }
        public int Category { get; set; }
        public string RankInfo { get; set; }
        public int Scid { get; set; }
        public string OriSongName { get; set; }
        public string Uploader { get; set; }
        public int SQBitrate { get; set; }
        public int HQBitrate { get; set; }
        public int Audioid { get; set; }
        public int HiFiQuality { get; set; }
        public string OriOtherName { get; set; }
        public int AlbumPrivilege { get; set; }
        public string TopicUrl { get; set; }
        public string SuperFileHash { get; set; }
        public int ASQPrivilege { get; set; }
        public int OldCpy { get; set; }
        public int IsOriginal { get; set; }
        public int Privilege { get; set; }
        public string TagContent { get; set; }
        public int ResBitrate { get; set; }
        public string FileHash { get; set; }
        public int SQPayType { get; set; }
        public Trans_Param1 trans_param { get; set; }
        public int HQPrice { get; set; }
        public int A320Privilege { get; set; }
        public string Type { get; set; }
        public string AlbumID { get; set; }
        public string AlbumName { get; set; }
        public string SQExtName { get; set; }
        public string vvid { get; set; }
        public string FileName { get; set; }
        public string MixSongID { get; set; }
        public string ID { get; set; }
        public int SuperFileSize { get; set; }
        public int SQPrivilege { get; set; }
        public string SQFileHash { get; set; }
        public string SuperExtName { get; set; }
        public int HQPrivilege { get; set; }
        public int SuperBitrate { get; set; }
        public int SuperDuration { get; set; }
        public int HQPkgPrice { get; set; }
        public string ResFileHash { get; set; }
        public int FileSize { get; set; }
        public int ResFileSize { get; set; }
        public string HQFileHash { get; set; }
        public string SongLabel { get; set; }
        public string PublishTime { get; set; }
        public int Publish { get; set; }
        public int mvTotal { get; set; }
        public string MvHash { get; set; }
        public int PkgPrice { get; set; }
        public int M4aSize { get; set; }
        public int Duration { get; set; }
        public string OtherName { get; set; }
        public int PublishAge { get; set; }
        public int SQPrice { get; set; }
        public int ResDuration { get; set; }
        public int[] SingerId { get; set; }
        public int Price { get; set; }
        public int FailProcess { get; set; }
        public string SingerName { get; set; }
        public int HQFailProcess { get; set; }
        public int HQFileSize { get; set; }
        public int HQPayType { get; set; }
        public int HQDuration { get; set; }
        public int PayType { get; set; }
        public int HasAlbum { get; set; }
        public int QualityLevel { get; set; }
        public int SourceID { get; set; }
    }

    public class Trans_Param1
    {
        public int cid { get; set; }
        public int pay_block_tpl { get; set; }
        public int musicpack_advance { get; set; }
        public int display_rate { get; set; }
        public Classmap1 classmap { get; set; }
        public int display { get; set; }
        public int cpy_attr0 { get; set; }
        public int cpy_grade { get; set; }
        public int cpy_level { get; set; }
        public string appid_block { get; set; }
        public Hash_Offset1 hash_offset { get; set; }
    }

    public class Classmap1
    {
        public int attr0 { get; set; }
    }

    public class Hash_Offset1
    {
        public int file_type { get; set; }
        public int start_byte { get; set; }
        public int start_ms { get; set; }
        public string offset_hash { get; set; }
        public int end_ms { get; set; }
        public int end_byte { get; set; }
    }


}
