using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Service.MessageService;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> AddMessage([FromBody] NewMessage request)
        {
            var message = await _messageService.AddMessage(request);
            return Ok(message);
        }

        [HttpGet("chatroom/{chatRoomId}")]
        public async Task<ActionResult<List<Message>>> GetMessagesByChatRoomId(string chatRoomId)
        {
            try
            {
                var messages = await _messageService.GetMessagesByChatRoomId(chatRoomId);
                if (messages is null) return NotFound();

                return Ok(messages);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Message>> GetMessageById(string Id)
        {
            try
            {
                var message = await _messageService.GetMessageById(Id);
                if (message is null) return NotFound();
                return Ok(message);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        //[HttpGet("filter/{chatRoomId?}/{pageSize?}/{lastMessageId?}")]
        [HttpGet("filter")]
        public async Task<ActionResult<List<Message>>> GetMessagesPageByChatRoomId(string? chatRoomId, int? pageSize, string? lastMessageId)
        {
            try
            {
                var messages = await _messageService.GetMessagesPageByChatRoomId(chatRoomId, pageSize, lastMessageId);
                if (messages is null) return NotFound();
                return messages;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpPut("delete/{messageId}")]
        public async Task<ActionResult> SetDeleteMessage(string messageId)
        {
            await _messageService.SetDeleteMessage(messageId);
            return NoContent();
        }

        [HttpPost("newfileupload")]
        public async Task<ActionResult<Message>> AddNewMessageForFileUpload(NewMessage request)
        {
            var message = await _messageService.AddNewMessageForFileUpload(request);
            return Ok(message);
        }

        [HttpPut("{messageId}/fileuploadfinished")]
        public async Task<ActionResult> UpdateMessageOnFileUploadFinish(string messageId, [FromBody] string fileUrls)
        {
            await _messageService.UpdateMessageOnFileUploadFinish(messageId, fileUrls);
            return NoContent();
        }

        [HttpPut("{messageId}/canceluploadingmessagefile")]
        public async Task<ActionResult> CancelUploadingMessageFile(string messageId)
        {
            await _messageService.CancelUploadingMessageFile(messageId);
            return NoContent();
        }

        [HttpGet("{lastMessageId}/getmissingmessages")]
        public async Task<ActionResult<Message>> GetMissingMessages(string lastMessageId)
        {
            var messages = await _messageService.GetMissingMessages(lastMessageId);
            return Ok(messages);
        }

        [HttpPost("AddNewMessageForAudioRecord")]
        public async Task<ActionResult<Message>> AddNewMessageForAudioRecord(NewMessage request)
        {
            var message = await _messageService.AddNewMessageForAudioRecord(request);
            return Ok(message);
        }

        [HttpPut("{messageId}/uploadfilemessageerror")]
        public async Task<ActionResult<Message>> UploadMessageFileError(string messageId)
        {
            var message = await _messageService.UploadMessageFileError(messageId);
            return Ok(message);
        }
    }
}
