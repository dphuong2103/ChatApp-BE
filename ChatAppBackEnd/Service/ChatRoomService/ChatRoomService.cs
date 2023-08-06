using AutoMapper;
using ChatAppBackEnd.Data;
using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;
using ChatAppBackEnd.Service.HubService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Service.ChatRoomService
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly DataContext _dbContext;
        private readonly IHubService _hubservice;
        private readonly IMapper _mapper;
        public ChatRoomService(DataContext dbContext, IHubService hubservice, IMapper mapper)
        {
            _dbContext = dbContext;
            _hubservice = hubservice;
            _mapper = mapper;
        }

        public async Task<ChatRoomSummary> AddChatRoom(NewChatRoomAndUserList newChatRoomAndUserList)
        {
            if (newChatRoomAndUserList.UserIds is null || newChatRoomAndUserList.UserIds.Count < 2)
            {
                throw new Exception("UserIDs is null");
            }
            string user1Id = newChatRoomAndUserList.UserIds[0];
            string user2Id = newChatRoomAndUserList.UserIds[1];
            var user1ChatRoomIds = await _dbContext.UserChatRooms.Where(ucr => ucr.UserId == user1Id && ucr.ChatRoom.ChatRoomType == "ONE").Select(ucr => ucr.ChatRoomId).ToListAsync();
           

            if (user1ChatRoomIds is not null)
            {
                var chatRoom2 = await _dbContext.UserChatRooms.Where(ucr => ucr.UserId == user2Id && user1ChatRoomIds.Contains(ucr.ChatRoomId)).FirstOrDefaultAsync();
                if (chatRoom2 is not null)
                {
                    throw new Exception("ChatRoom already exists");
                }
            }
            var chatRoom = _mapper.Map<ChatRoom>(newChatRoomAndUserList.NewChatRoom);

            _dbContext.ChatRooms.Add(chatRoom);
            await _dbContext.SaveChangesAsync();

            if (string.IsNullOrEmpty(chatRoom.Id))
            {
                throw new InvalidOperationException("chatroomId does not exist");
            }

            foreach (var userId in newChatRoomAndUserList.UserIds)
            {
                var ucr = new UserChatRoom { ChatRoomId = chatRoom.Id, UserId = userId, CreatedTime = chatRoom.CreatedTime };
                _dbContext.UserChatRooms.Add(ucr);
            }
            await _dbContext.SaveChangesAsync();
            var users = await _dbContext.Users.Where(user => newChatRoomAndUserList.UserIds.Contains(user.Id)).ToListAsync();
            var chatRoomSummary = new ChatRoomSummary { ChatRoom = chatRoom, Users = users };
            await _hubservice.SendChatRoomInfo(chatRoomSummary);
            var userChatRoom = await _dbContext.UserChatRooms.FirstOrDefaultAsync(ucr => ucr.ChatRoomId == chatRoom.Id && ucr.UserId == chatRoom.CreatorId);
            if (userChatRoom is not null) chatRoomSummary.UserChatRoom = userChatRoom;
            return chatRoomSummary;
        }

        public async Task<List<ChatRoom>> GetAllChatRooms()
        {
            var allChatRooms = await _dbContext.ChatRooms.ToListAsync();
            return allChatRooms;
        }

        public async Task<List<ChatRoomSummary>> GetChatRoomSummariesByUserId(string userId)
        {

            var userChatRooms = await _dbContext.UserChatRooms
                .Include(ucr => ucr.ChatRoom)
                .Where(ucr => ucr.UserId == userId && ucr.MembershipStatus == UserChatRoomStatus.Active)
                .ToListAsync();

            var chatRoomIds = userChatRooms.Select(ucr => ucr.ChatRoomId).ToList();

            var latestMessages = await _dbContext.Messages
                .Where(m => chatRoomIds.Contains(m.ChatRoomId))
                .GroupBy(m => m.ChatRoomId)
                .Select(g => g.OrderByDescending(m => m.CreatedTime).FirstOrDefault())
                .ToListAsync();

            var chatRoomSummaries = new List<ChatRoomSummary>();

            foreach (var userChatRoom in userChatRooms)
            {
                var chatRoom = userChatRoom.ChatRoom;
                var latestMessage = latestMessages
                    .FirstOrDefault(m => m?.ChatRoomId == userChatRoom.ChatRoomId);
                int unreadCount;
                var lastMessageRead = await _dbContext.Messages.FindAsync(userChatRoom.LastMessageReadId);
                if (lastMessageRead is null)
                {
                    unreadCount = await _dbContext.Messages
                   .CountAsync(m => m.ChatRoomId == userChatRoom.ChatRoomId && m.CreatedTime > userChatRoom.CreatedTime);
                }
                else
                {
                    unreadCount = await _dbContext.Messages.CountAsync(m => m.ChatRoomId == lastMessageRead.ChatRoomId && m.CreatedTime >= lastMessageRead.CreatedTime && m.Id != lastMessageRead.Id);
                }

                var usersInChatRoom = await _dbContext.UserChatRooms
                    .Where(ucr => ucr.ChatRoomId == userChatRoom.ChatRoomId && ucr.MembershipStatus == UserChatRoomStatus.Active)
                    .Select(ucr => ucr.User)
                    .ToListAsync();

                chatRoomSummaries.Add(new ChatRoomSummary
                {
                    UserChatRoom = userChatRoom,
                    ChatRoom = chatRoom,
                    LatestMessage = latestMessage,
                    Users = usersInChatRoom,
                    NumberOfUnreadMessages = unreadCount
                });
            }

            return chatRoomSummaries;
        }

        public async Task<ChatRoom?> GetChatRoomById(string Id)
        {
            var chatRoom = await _dbContext.ChatRooms.FindAsync(Id);
            return chatRoom;
        }

        public async Task<ChatRoom?> UpdateChatRoom(string Id, ChatRoom request)
        {
            var chatRoom = await GetChatRoomById(Id);
            if (chatRoom == null) return null;
            _dbContext.Entry(chatRoom).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            return chatRoom;
        }

        public async Task<ChatRoomSummary> AddChatRoomGroup(NewChatRoomAndUserList request)
        {
            var chatRoom = _mapper.Map<ChatRoom>(request.NewChatRoom);
            chatRoom.ChatRoomType = ChatRoom.CHATROOMTYPE_MANY;
            chatRoom.UpdatedTime = chatRoom.CreatedTime;
            _dbContext.ChatRooms.Add(chatRoom);
            await _dbContext.SaveChangesAsync();

            if (string.IsNullOrEmpty(chatRoom.Id))
            {
                throw new InvalidOperationException("chatroomId does not exist");
            }
            foreach (var userId in request.UserIds)
            {
                var ucr = new UserChatRoom { ChatRoomId = chatRoom.Id, UserId = userId, CreatedTime = chatRoom.CreatedTime };
                _dbContext.UserChatRooms.Add(ucr);
            }
            await _dbContext.SaveChangesAsync();
            var users = await _dbContext.Users.Where(user => request.UserIds.Contains(user.Id)).ToListAsync();
            var chatRoomSummary = new ChatRoomSummary { ChatRoom = chatRoom, Users = users };
            await _hubservice.SendChatRoomInfo(chatRoomSummary);
            var userChatRoom = await _dbContext.UserChatRooms.FirstOrDefaultAsync(ucr => ucr.ChatRoomId == chatRoom.Id && ucr.UserId == chatRoom.CreatorId);
            if (userChatRoom is not null) chatRoomSummary.UserChatRoom = userChatRoom;
            return chatRoomSummary;
        }

        public async Task<ChatRoom> UpdateChatRoomName(string chatRoomId, ChatRoomIdAndName request)
        {
            var chatRoom = await _dbContext.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom is null)
            {
                throw new Exception("ChatRoom is null");
            }
            chatRoom.Name = request.Name;
            await _dbContext.SaveChangesAsync();
            await _hubservice.UpdateChatRoomName(request);
            return chatRoom;
        }
    }
}
