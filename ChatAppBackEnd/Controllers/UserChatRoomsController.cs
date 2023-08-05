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
            try
            {
                var userChatRooms = await _userChatRoomService.GetUserChatRoomsByUserId(userId);
                if (userChatRooms is null) return NotFound();
                return Ok(userChatRooms);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{userChatRoomId}")]
        public async Task<ActionResult<UserChatRoom>> UpdateUserChatRoomLastMessageRead(string userChatRoomId, UpdateLastMessageRead request)
        {
            try
            {
                await _userChatRoomService.UpdateUserChatRoomLastMessageRead(userChatRoomId, request);
                return NoContent();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{userChatRoomId}/setmuted")]
        public async Task<ActionResult<UserChatRoom>> SetMuted(string userChatRoomId, SetMutedDTO request)
        {
            try
            {
                var userChatRoom = await _userChatRoomService.SetMuted(userChatRoomId, request);
                return Ok(userChatRoom);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpPost("addmembers")]
        public async Task<ActionResult> AddMembersToChatGroup(ChatRoomIdAndUserIds request)
        {
            try
            {
                await _userChatRoomService.AddMembersToChatGroup(request);
                return NoContent();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpPut("removefromgroupchat")]
        public async Task<ActionResult> RemoveMemberFromGroupChat(UserIdAndChatRoomId request)
        {
            try
            {
                var userChatRoom = await _userChatRoomService.RemoveMemberFromGroupChat(request);
                if (userChatRoom is null) return NotFound();
                return NoContent();
            }catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }
    }
}
