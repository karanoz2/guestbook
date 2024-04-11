namespace test.book.BLL.MessagingFeature.Exceptions
{
    public class MessageAddingException : ApplicationException
    {
        public MessageAddingException(string? message) : base(message) { }
        public MessageAddingException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
