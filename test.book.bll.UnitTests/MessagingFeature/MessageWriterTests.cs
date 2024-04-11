using Moq;
using test.book.BLL.MessagingFeature;
using test.book.BLL.MessagingFeature.Contracts;
using test.book.BLL.MessagingFeature.Exceptions;
using test.book.DAL;
using test.book.DAL.Entities;

namespace test.book.bll.UnitTests.MessagingFeature
{
    public class MessageWriterTests
    {
        private readonly IMessageInsert _messageInsert;
        private readonly Mock<IMessageManager> _dalMessages;
        public MessageWriterTests()
        {
            _dalMessages = new Mock<IMessageManager>();
            _messageInsert = new MessageMutator(_dalMessages.Object);
        }

        [Fact]
        public async Task SuccessFlow()
        {
            // Arrange.
            var participant = "Vasy";
            var text = "It was excelent!";

            // Act.
            var list = await _messageInsert.Insert(new BLL.MessagingFeature.Contracts.JMessageCreate
            {
                Participant = participant,
                Text = text
            });

            // Assert.
            _dalMessages.Verify(d => d.AddMessage(It.Is<Message>(m => m.Participant == participant && m.Text == text)));
        }


        [Fact]
        public async Task WrongParameters_null()
        {
            // Arrange.
            JMessageCreate message = null;

            // Act.
            Task result() => _messageInsert.Insert(message);

            // Assert.
            await Assert.ThrowsAsync<ArgumentNullException>(result);
        }

        [Theory]
        [InlineData(null, "Hello")]
        [InlineData("Vasya", null)]
        [InlineData(null, null)]
        [InlineData("", "Hello")]
        [InlineData("Vasya", "")]
        [InlineData("", "")]
        [InlineData("VasyaVasyaVasyaVasyaVasyaVasya", "Hello")]
        [InlineData("Vasya", "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello")]
        public async Task WrongParameters_wrongProperties(string participant, string text)
        {
            // Arrange.
            JMessageCreate message = new JMessageCreate { 
                Participant = participant,
                Text = text
            };

            // Act.
            Task result() => _messageInsert.Insert(message);

            // Assert.
            await Assert.ThrowsAsync<MessageValidateionException>(result);
        }
    }
}
