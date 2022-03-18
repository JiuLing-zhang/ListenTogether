using System.Diagnostics.CodeAnalysis;

//TODO 将文件移动到根目录，重命名
namespace MusicPlayerOnline.Model.ApiResponse
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

    public class Result<T> where T : class, new()
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
