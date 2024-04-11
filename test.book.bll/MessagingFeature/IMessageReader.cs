using test.book.BLL.MessagingFeature.Contracts;

namespace test.book.BLL.MessagingFeature
{
    public interface IMessageReader
    {
        Task<JPageOfMessages> ReadPage(int perpage = 10, int page = 0);
    }
}