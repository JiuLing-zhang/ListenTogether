namespace MusicPlayerOnline.Model
{
    public class Result
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class Result<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public Result(int code, string message, T? data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
