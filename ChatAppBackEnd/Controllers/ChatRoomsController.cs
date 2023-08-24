using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Service.ChatRoomService;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;
        public ChatRoomsController(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ChatRoomSummary>>> GetChatRoomAndLastMessagesByUserId(string userId)
        {
            try
            {
                var chatRoomLatestMessageAndUserList = await _chatRoomService.GetChatRoomSummariesByUserId(userId);
                return Ok(chatRoomLatestMessageAndUserList);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ChatRoomSummary>> AddChatRoom([FromBody] NewChatRoomAndUserList newChatRoomAndUserList)
        {

            var chatRoomSummary = await _chatRoomService.AddChatRoom(newChatRoomAndUserList);
            return Ok(chatRoomSummary);

        }

        [HttpPost("newchatgroup")]
        public async Task<ActionResult<ChatRoomSummary>> AddChatRoomGroup([FromBody] NewChatRoomAndUserList newChatRoomAndUserList)
        {
            var chatRoomSummary = await _chatRoomService.AddChatRoomGroup(newChatRoomAndUserList);
            return Ok(chatRoomSummary);
        }

        [HttpPut("{chatRoomId}/updatechatname")]
        public async Task<ActionResult> UpdateChatRoomName(string chatRoomId, ChatRoomIdAndName request)
        {
            await _chatRoomService.UpdateChatRoomName(chatRoomId, request);
            return NoContent();
        }

        [HttpPut("{chatRoomId}/updatechatroomavatar")]
        public async Task<ActionResult> UpdateChatRoomAvatar(string chatRoomId, ChatRoomIdAndImageUrl request)
        {
            await _chatRoomService.UpdateChatRoomAvatar(chatRoomId, request);
            return NoContent();
        }
    }

}
