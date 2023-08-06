using ChatAppBackEnd.Data;
using ChatAppBackEnd.Hubs;
using ChatAppBackEnd.Hubs.Service;
using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Service.HubService
{
    public class HubService : IHubService
    {
        private readonly IUserConnectionIdService _userConnectionIdService;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;
        private readonly DataContext _dbContext;
        public HubService(IUserConnectionIdService userConnectionIdService, IHubContext<ChatHub, IChatHub> hubContext, DataContext dbContext)
        {
            _userConnectionIdService = userConnectionIdService;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task SendRelationship(UserRelationship userRelationship, string type)
        {
            var connectionIds = _userConnectionIdService.GetUsersConnectionIdsByUserIdList(new List<string>() { userRelationship.TargetUserId, userRelationship.InitiatorUserId });
            if (connectionIds is not null)
            {
                await _hubContext.Clients.Clients(connectionIds).ReceivedRelationship(userRelationship, type);
            }
        }

        public async Task SendChatRoomInfo(ChatRoomSummary chatRoomSummary)
        {
            var connectionIds = _userConnectionIdService.GetUsersConnectionIds(chatRoomSummary.Users);
            if (connectionIds is null) return;
            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, chatRoomSummary.ChatRoom.Id!);
            }
            foreach (var user in chatRoomSummary.Users)
            {
                var userChatRoom = _dbContext.UserChatRooms.FirstOrDefault(ucr => ucr.ChatRoomId == chatRoomSummary.ChatRoom.Id && ucr.UserId == user.Id);
                if (userChatRoom is not null) chatRoomSummary.UserChatRoom = userChatRoom;
                var userConnectionIds = _userConnectionIdService.GetUsersConnectionIdsByUserId(user.Id);
                if (userConnectionIds is null) continue;
                foreach (var connectionId in userConnectionIds)
                {
                    await _hubContext.Clients.Client(connectionId).ReceivedChatroom(chatRoomSummary);
                }
            }
        }

        public async Task SendMessage(Message message)
        {
            await _hubContext.Clients.Groups(message.ChatRoomId).ReceivedMessage(message);

        }

        public async Task RemoveUserFromGroupChat(UserIdAndChatRoomId request)
        {
            await _hubContext.Clients.Group(request.ChatRoomId).RemoveUserFromGroupChat(request);
            var connectionIds = _userConnectionIdService.GetUsersConnectionIdsByUserId(request.UserId);
            if (connectionIds is null) return;
            foreach (string connectionId in connectionIds)
            {
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, request.ChatRoomId);
            }
        }

        public async Task AddMembersToChatGroup(ChatRoomIdAndUsers request)
        {
            var chatRoom = await _dbContext.ChatRooms.FindAsync(request.ChatRoomId);
            var latestMessage = await _dbContext.Messages.Where(m => m.ChatRoomId == request.ChatRoomId).OrderByDescending(m => m.CreatedTime).FirstOrDefaultAsync();
            var users = await _dbContext.UserChatRooms.Include(ucr => ucr.User).Where(ucr => ucr.ChatRoomId == request.ChatRoomId && ucr.MembershipStatus == UserChatRoomStatus.Active).Select(ucr => ucr.User).ToListAsync();
            if (chatRoom is null) return;
            foreach (var user in request.Users)
            {
                var connectionIds = _userConnectionIdService.GetUsersConnectionIdsByUserId(user.Id);
                if (connectionIds is null) continue;
                var userChatRoom = await _dbContext.UserChatRooms.Where(ucr => ucr.UserId == user.Id && ucr.ChatRoomId == request.ChatRoomId && ucr.MembershipStatus == UserChatRoomStatus.Active).FirstOrDefaultAsync();
                if (userChatRoom is null) {
                    throw new Exception("User chatroom is null");
                };
                ChatRoomSummary crs = new ChatRoomSummary { ChatRoom = chatRoom, LatestMessage = latestMessage, Users = users, UserChatRoom = userChatRoom };
                await _hubContext.Clients.Clients(connectionIds).ReceivedChatroom(crs);
            }
            await _hubContext.Clients.Group(request.ChatRoomId).AddMembersToChatRoom(request);
            await AddUserConnectionIdsToChatGroup(request.Users, request.ChatRoomId);
        }

        public async Task AddUserConnectionIdsToChatGroup(List<User> users, string chatRoomId)
        {
            var connectionIds = _userConnectionIdService.GetUsersConnectionIds(users);
            if (connectionIds is null) return;
            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, chatRoomId);
            }
        }

        public async Task UpdateChatRoomName(ChatRoomIdAndName request)
        {
            await _hubContext.Clients.Groups(request.ChatRoomId).UpdateChatRoomName(request);
        }
    }
}
