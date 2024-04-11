using test.book.DAL.Entities;

namespace test.book.DAL
{
    public interface IMessageManager
    {
        Task<int> GetCount();

        Task<IEnumerable<Message>> GetMessages(int perpage, int page);

        Task<int> AddMessage(Message mess);
    }
}