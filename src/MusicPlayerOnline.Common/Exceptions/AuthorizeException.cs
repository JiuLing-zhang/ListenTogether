namespace MusicPlayerOnline.Common.Exceptions;

public class AuthorizeException : Exception
{
    public AuthorizeException() : base() { }

    public AuthorizeException(string message) : base(message) { }

    public AuthorizeException(string message, params object[] args)
        : base(String.Format(message, args))
    {
    }
}