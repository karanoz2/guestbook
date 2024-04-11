namespace test.book.BLL.MessagingFeature.Contracts
{
    public class JPageOfMessages
    {
        public IEnumerable<JMessage> Messages { get; set; }

        public int Count { get; set; }
    }
}
