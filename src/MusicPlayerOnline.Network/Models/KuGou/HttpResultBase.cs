namespace MusicPlayerOnline.Network.Models.KuGou
{
    public class HttpResultBase<T>
    {
        public int status { get; set; }
        public int error_code { get; set; }
        public string error_msg { get; set; }
        public T data { get; set; }
    }
}
