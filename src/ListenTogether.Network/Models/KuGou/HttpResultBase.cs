namespace ListenTogether.Network.Models.KuGou
{
    public class HttpResultBase<T>
    {
        public int status { get; set; }
        public int error_code { get; set; }
        public string error_msg { get; set; } = null!;
        public T data { get; set; } = default(T)!;
    }
}
