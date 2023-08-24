namespace ChatAppBackEnd.Exceptions
{
    public class EmptyException : Exception
    {
        public EmptyException(string message) : base(message) { }
    }

    public sealed class EmptyFileUrlException : EmptyException
    {
        public EmptyFileUrlException() : base("Fileurl is empty") { }
    }
    public sealed class EmptyMessageException : EmptyException
    {
        public EmptyMessageException() : base("Message text is empty") { }
    }
}
