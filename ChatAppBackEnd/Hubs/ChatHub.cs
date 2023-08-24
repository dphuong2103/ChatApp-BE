using ChatAppBackEnd.Data;
using ChatAppBackEnd.Hubs.Service;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly DataContext _dbContext;
        private readonly string OPENCHATROOM = "OPENCHATROOM";
        private readonly IUserConnectionIdService _userConnectionIdService;
        public ChatHub(DataContext dbContext, IUserConnectionIdService userConnectionIdService)
        {
            _dbContext = dbContext;
            _userConnectionIdService = userConnectionIdService;
        }


        public async Task UserOnline(string userId)
        {
            try
            {
                _userConnectionIdService.Add(userId, Context.ConnectionId);
                var chatRoomIds = await _dbContext.UserChatRooms.Where(ucr => ucr.UserId == userId).Select(cr => cr.ChatRoomId).ToListAsync();
                if (chatRoomIds is null) return;
                foreach (var chatRoomId in chatRoomIds)
                {
                    if (chatRoomId is null) continue;
                    await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
                }

                await Clients.Client(Context.ConnectionId).UpdateConnectionId(Context.ConnectionId);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public async Task CloseChatRoom(string chatRoomId)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
        public async Task OpenChatRoom(string chatRoomId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, OPENCHATROOM + chatRoomId);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.ConnectionId is not null)
            {
                _userConnectionIdService.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task InitiateCall(CallData callData)
        {
            callData.FromConnectionID = Context.ConnectionId;
            await Clients.GroupExcept(callData.ChatRoomID, callData.FromConnectionID).ReceiveCall(callData);
        }

        public async Task AcceptCall(object signalData, string toConnectionId)
        {
            try
            {
                await Clients.Client(toConnectionId).CallAccepted(signalData);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public async Task LeaveCall(string? chatRoomId, string? userId)
        {
            if (string.IsNullOrEmpty(chatRoomId) || string.IsNullOrEmpty(userId)) return;

            try
            {
                await Clients.GroupExcept(chatRoomId, Context.ConnectionId).LeavedCall(userId);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
        public async Task DeclinedCall(string fromConnectionId) {
            await Clients.Clients(fromConnectionId).CallDeclined(fromConnectionId);
        }

    
    }
}
