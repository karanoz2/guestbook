using test.book.BLL.MessagingFeature.Contracts;
using test.book.BLL.MessagingFeature.Exceptions;
using test.book.BLL.MessagingFeature.Mapping;
using test.book.DAL;

namespace test.book.BLL.MessagingFeature
{
    public class MessageReader(IMessageManager manager) : IMessageReader
    {
        private readonly IMessageManager _messageReader = manager;

        public async Task<JPageOfMessages> ReadPage(int perpage = 10, int page = 0)
        {
            if (perpage < 1 || perpage > 50) throw new MessageValidateionException("invalid 'perpage' parameter");
            if (page < 0) throw new MessageValidateionException("invalid 'page' parameter");

            var messages = await _messageReader.GetMessages(perpage, page);
            var count = await _messageReader.GetCount();

            return new JPageOfMessages
            {
                Messages = messages.Select(_ => _.ToContract()),
                Count = count
            };
        }
    }
}
