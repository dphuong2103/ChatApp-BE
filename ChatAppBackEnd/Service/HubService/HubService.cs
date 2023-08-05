using ChatAppBackEnd.Data;
using ChatAppBackEnd.Hubs;
using ChatAppBackEnd.Hubs.Service;
using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;
using Microsoft.AspNetCore.SignalR;

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
            await AddUserConnectionIdsToChatGroup(request.Users, request.ChatRoomId);
            await _hubContext.Clients.Group(request.ChatRoomId).AddMembersToChatRoom(request);
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
