using Microsoft.AspNetCore.Mvc;
using test.book.BLL.MessagingFeature;
using test.book.BLL.MessagingFeature.Contracts;
using test.book.BLL.MessagingFeature.Exceptions;

namespace test.book.Controllers
{
    [ApiController]
    [Route("guestbook")]
    public class GuestbookController : ControllerBase
    {
        private readonly IMessageReader _messageReader;
        private readonly IMessageInsert _messageInserter;
        public GuestbookController(IMessageReader messageReader, IMessageInsert messageInserter) {
            _messageReader = messageReader;
            _messageInserter = messageInserter;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
           return Ok("pong");
        }


        [HttpGet("")]
        public async Task<IActionResult> Get(int p = 0, int pp = 10)
        {
            try
            {
                return Ok(await _messageReader.ReadPage(pp, p));
            }
            catch (MessageValidateionException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(JMessageCreate data)
        {
            try
            {
                return Ok(await _messageInserter.Insert(data));
            }
            catch (MessageValidateionException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
