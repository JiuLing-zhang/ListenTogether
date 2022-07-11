namespace ListenTogether.Network.Models.KuWo
{
    internal class HttpResultBase<T>
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
