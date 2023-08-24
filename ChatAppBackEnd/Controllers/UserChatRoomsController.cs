using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;
using ChatAppBackEnd.Service.UserChatRoomService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserChatRoomsController : ControllerBase
    {
        private readonly IUserChatRoomService _userChatRoomService;
        public UserChatRoomsController(IUserChatRoomService userChatRoomService)
        {
            _userChatRoomService = userChatRoomService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<UserChatRoom>>> GetUserChatRoomsByUserId(string userId)
        {

            var userChatRooms = await _userChatRoomService.GetUserChatRoomsByUserId(userId);
            if (userChatRooms is null) return NotFound();
            return Ok(userChatRooms);

        }

        [HttpPut("{userChatRoomId}")]
        public async Task<ActionResult<UserChatRoom>> UpdateUserChatRoomLastMessageRead(string userChatRoomId, UpdateLastMessageRead request)
        {
            await _userChatRoomService.UpdateUserChatRoomLastMessageRead(userChatRoomId, request);
            return NoContent();
        }

        [HttpPut("{userChatRoomId}/setmuted")]
        public async Task<ActionResult<UserChatRoom>> SetMuted(string userChatRoomId, SetMutedDTO request)
        {
            var userChatRoom = await _userChatRoomService.SetMuted(userChatRoomId, request);
            return Ok(userChatRoom);
        }

        [HttpPost("addmembers")]
        public async Task<ActionResult> AddMembersToChatGroup(ChatRoomIdAndUserIds request)
        {
            await _userChatRoomService.AddMembersToChatGroup(request);
            return NoContent();
        }

        [HttpPut("removefromgroupchat")]
        public async Task<ActionResult> RemoveMemberFromGroupChat(UserIdAndChatRoomId request)
        {
            var userChatRoom = await _userChatRoomService.RemoveMemberFromGroupChat(request);
            if (userChatRoom is null) return NotFound();
            return NoContent();
        }

        [HttpPut("{userChatRoomId}/leavechatroom")]
        public async Task<ActionResult> LeaveChatRoom(string userChatRoomId)
        {
            await _userChatRoomService.LeaveChatRoom(userChatRoomId);
            return NoContent();
        }
    }
}
