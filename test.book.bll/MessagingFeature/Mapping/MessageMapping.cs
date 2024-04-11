using test.book.BLL.MessagingFeature.Contracts;
using test.book.DAL.Entities;


namespace test.book.BLL.MessagingFeature.Mapping
{
    public static class MessageMapping
    {
        public static JMessage ToContract(this Message data, int? id = null)
        {
            return new JMessage
            {
                Id = id ?? data.Id,
                Participant = data.Participant,
                Text = data.Text,
                Created = data.Created
            };
        }

        public static Message ToEntity(this JMessageCreate data)
        {
            ArgumentNullException.ThrowIfNull(data);
            return new Message
            {
                Id = default,
                Participant = data.Participant,
                Text = data.Text,
                Created = default
            };
        }
    }
}
