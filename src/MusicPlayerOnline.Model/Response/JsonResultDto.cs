namespace MusicPlayerOnline.Model.Response
{
    public class JsonResultDto
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public JsonResultDto(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
