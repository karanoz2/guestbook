using test.book.BLL.MessagingFeature.Contracts;
using test.book.BLL.MessagingFeature.Exceptions;
using test.book.BLL.MessagingFeature.Mapping;
using test.book.DAL;

namespace test.book.BLL.MessagingFeature
{
    public class MessageMutator : IMessageInsert
    {
        private readonly IMessageManager _messageReader;
        public MessageMutator(IMessageManager manager)
        {
            _messageReader = manager;
        }

        public async Task<JMessage> Insert(JMessageCreate data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (string.IsNullOrEmpty(data.Participant)) throw new MessageValidateionException("participant required");
            if (string.IsNullOrEmpty(data.Text)) throw new MessageValidateionException("message required");
            if (data.Text.Length > 100) throw new MessageValidateionException("max length of message 100 characters");
            if (data.Participant.Length > 20) throw new MessageValidateionException("max length of participant-name 20 characters");

            var entity = data.ToEntity();
            var id = await _messageReader.AddMessage(data.ToEntity());

            return entity.ToContract(id) ?? throw new MessageAddingException("add message error");
        }
    }
}
