using ChatAppBackEnd.Data;
using ChatAppBackEnd.Exceptions;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;
using ChatAppBackEnd.Service.HubService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Service.UserChatRoomService
{
    public class UserChatRoomService : IUserChatRoomService
    {
        private readonly DataContext _dbContext;
        private readonly IHubService _hubService;
        public UserChatRoomService(DataContext dbContext, IHubService hubService)
        {
            _dbContext = dbContext;
            _hubService = hubService;
        }
        public async Task<UserChatRoom> AddUserChatRoom(UserChatRoom request)
        {
            _dbContext.Add(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<List<UserChatRoom>?> GetUserChatRoomsByUserId(string UserId)
        {
            var userChatRooms = await _dbContext
                .UserChatRooms
                .Where(userChatRoom => userChatRoom.UserId == UserId)
                .Include(userChatRoom => userChatRoom.ChatRoom)
                .ToListAsync();

            return userChatRooms;
        }

        public async Task<UserChatRoom> UpdateUserChatRoomLastMessageRead(string userChatRoomId, UpdateLastMessageRead request)
        {
            if (request.LastMessageReadId is null)
            {
                throw new Exception("Last messagereadid is null");
            };
            var userChatRoom = await _dbContext.UserChatRooms.FindAsync(userChatRoomId);
            if (userChatRoom is null) {
                throw NotFoundUserChatRoomException.ById(userChatRoomId);
            };
            userChatRoom.LastMessageReadId = request.LastMessageReadId;
            await _dbContext.SaveChangesAsync();
            return userChatRoom;
        }

        public async Task<UserChatRoom> SetMuted(string userChatRoomId, SetMutedDTO request)
        {
            var userChatRoom = await _dbContext.UserChatRooms.FindAsync(userChatRoomId);
            if (userChatRoom is null) {
                throw NotFoundUserChatRoomException.ById(userChatRoomId);
            }
            userChatRoom.IsMuted = request.IsMuted;
            await _dbContext.SaveChangesAsync();
            return userChatRoom;
        }

        public async Task AddMembersToChatGroup(ChatRoomIdAndUserIds request)
        {
            if (request.UserIds.Count == 0) return;
            var chatRoom = await _dbContext.ChatRooms.FindAsync(request.ChatRoomId);
            if (chatRoom is null)
            {
                return;
            }
            if (chatRoom.ChatRoomType == ChatRoomType.ONE)
            {
                return;
            }
            DateTime createdTime = DateTime.UtcNow;
            foreach (string userId in request.UserIds)
            {
                var userChatRoom = await _dbContext.UserChatRooms.Where(ucr => ucr.ChatRoomId == request.ChatRoomId && ucr.UserId == userId).FirstOrDefaultAsync();
                if (userChatRoom is not null)
                {
                    userChatRoom.MembershipStatus = UserChatRoomStatus.Active;
                }
                else
                {
                    userChatRoom = new UserChatRoom { ChatRoomId = request.ChatRoomId, UserId = userId, CreatedTime = createdTime, MembershipStatus = UserChatRoomStatus.Active };
                    await _dbContext.UserChatRooms.AddAsync(userChatRoom);
                }
            }
            await _dbContext.SaveChangesAsync();
            var addedUsers = await _dbContext.Users.Where(user => request.UserIds.Contains(user.Id)).ToListAsync();
            ChatRoomIdAndUsers chatRoomIdAndUsers = new()
            {
                ChatRoomId = request.ChatRoomId,
                Users = addedUsers
            };
            await _hubService.AddMembersToChatGroup(chatRoomIdAndUsers);
        }

        public async Task<UserChatRoom> RemoveMemberFromGroupChat(UserIdAndChatRoomId request)
        {
            var userChatRoom = await _dbContext.UserChatRooms.Where(ucr => ucr.ChatRoomId == request.ChatRoomId && ucr.UserId == request.UserId && ucr.MembershipStatus == UserChatRoomStatus.Active).FirstOrDefaultAsync();
            if (userChatRoom is null)
            {
                throw NotFoundUserChatRoomException.ByChatRoomIdAndUserId(request.UserId, request.ChatRoomId);
            }
            userChatRoom.MembershipStatus = UserChatRoomStatus.Kicked;
            var chatRoom = await _dbContext.ChatRooms.FindAsync(request.ChatRoomId);
            if (chatRoom is null)
            {
                throw new NotFoundChatRoomException(request.ChatRoomId);
            }
            await _dbContext.SaveChangesAsync();
            var users = await _dbContext.UserChatRooms.Where(ucr => ucr.ChatRoomId == request.ChatRoomId).Include(ucr => ucr.User).Select(ucr => ucr.User).ToListAsync();
            await _hubService.RemoveUserFromGroupChat(request);
            return userChatRoom;
        }

        public async Task<UserChatRoom> LeaveChatRoom(string userChatRoomId)
        {
            var userChatRoom = await _dbContext.UserChatRooms.FindAsync(userChatRoomId);
            if (userChatRoom is null)
            {
                throw NotFoundUserChatRoomException.ById(userChatRoomId);
            }
            userChatRoom.MembershipStatus = UserChatRoomStatus.Left;
            await _dbContext.SaveChangesAsync();
            await _hubService.RemoveUserFromGroupChat(new UserIdAndChatRoomId() { ChatRoomId = userChatRoom.ChatRoomId, UserId = userChatRoom.UserId });
            return userChatRoom;
        }
    }
}
