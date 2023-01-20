namespace ListenTogether.Model.Api.Response
{
    public class MusicResponse : LocalMusic
    {
        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; } = null!;
    }
}
