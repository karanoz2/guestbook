using test.book.BLL.MessagingFeature.Contracts;

namespace test.book.BLL.MessagingFeature
{
    public interface IMessageInsert
    {
        Task<JMessage> Insert(JMessageCreate data);
    }
}