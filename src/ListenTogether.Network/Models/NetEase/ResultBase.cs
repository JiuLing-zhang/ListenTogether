namespace ListenTogether.Network.Models.NetEase
{
    public class ResultBase<T>
    {
        public List<T> data { get; set; }
        public T result { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
    }
}
