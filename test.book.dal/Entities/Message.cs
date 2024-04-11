namespace test.book.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Participant { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.MinValue;
    }
}
