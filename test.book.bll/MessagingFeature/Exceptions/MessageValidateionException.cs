namespace test.book.BLL.MessagingFeature.Exceptions
{
    public class MessageValidateionException : ApplicationException
    {
        public MessageValidateionException(string? message) : base(message) { }
        public MessageValidateionException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
