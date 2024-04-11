using Moq;
using test.book.BLL.MessagingFeature;
using test.book.BLL.MessagingFeature.Exceptions;
using test.book.DAL;
using test.book.DAL.Entities;

namespace test.book.bll.UnitTests.MessagingFeature
{
    public class MessageReaderTests
    {
        private readonly IMessageReader _messageReader;
        private readonly Mock<IMessageManager> _dalMessages;
        public MessageReaderTests() {
            _dalMessages = new Mock<IMessageManager>();
            _messageReader = new MessageReader(_dalMessages.Object);
        }

        [Fact]
        public async Task NoMessages() {
            // Arrange.
            _dalMessages.Setup(s => s.GetMessages(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Message[0]);

            // Act.
            var list = await _messageReader.ReadPage();

            // Assert.
            Assert.NotNull(list);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task FewMessages()
        {
            // Arrange.
            _dalMessages.Setup(s => s.GetMessages(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Message[2] { 
                new Message { Id = 1, Text = "Super!", Participant = "Billi", Created = DateTime.UtcNow },
                new Message { Id = 2, Text = "Excelent!", Participant = "Bob", Created = DateTime.UtcNow }
            });
            _dalMessages.Setup(s=>s.GetCount()).ReturnsAsync(2);

            // Act.
            var list = await _messageReader.ReadPage();

            // Assert.
            Assert.NotNull(list);
            Assert.Equal(2, list.Count);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(51, 0)]
        [InlineData(10, -1)]
        public async Task WrongPerPage1(int pp, int p)
        {
            // Arrange.
            _dalMessages.Setup(s => s.GetMessages(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Message[0]);

            // Act.
            Task result() => _messageReader.ReadPage(pp, p);

            // Assert.
            await Assert.ThrowsAsync<MessageValidateionException>(result);
        }

       
    }
}
