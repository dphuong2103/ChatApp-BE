using AutoMapper;
using ChatAppBackEnd.Data;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Service.HubService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Service.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _dbContext;
        private readonly IHubService _hubservice;
        private readonly IMapper _mapper;
        public MessageService(DataContext dbContext, IHubService hubservice, IMapper mapper)
        {
            _dbContext = dbContext;
            _hubservice = hubservice;
            _mapper = mapper;
        }
        public async Task<Message> AddMessage(NewMessage newMessage)
        {
            var message = _mapper.Map<Message>(newMessage);
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
            message = _dbContext.Messages.Include(m => m.Sender).FirstOrDefault(m => m.Id == message.Id);
            await _hubservice.SendMessage(message!);
            return message!;
        }

        public async Task<Message?> SetDeleteMessage(string messageId)
        {
            var message = _dbContext.Messages.Include(message => message.Sender).FirstOrDefault(message => message.Id == messageId);
            if (message is null) return message;
            message.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            await _hubservice.SendMessage(message);
            return message;
        }

        public async Task<Message?> GetMessageById(string Id)
        {
            var message = await _dbContext.Messages.FindAsync(Id);
            return message;
        }

        public async Task<List<Message>?> GetMessagesByChatRoomId(string chatRoomId)
        {
            var messagesByChatRoomId = await _dbContext.Messages.Where(message => message.ChatRoomId == chatRoomId)
                .Include(message => message.Sender).OrderByDescending(message => message.CreatedTime)
                .ToListAsync();
            return messagesByChatRoomId;
        }

        public async Task<List<Message>?> GetMessagesPageByChatRoomId(string? chatRoomId, int? pageSize = 2, string? lastMessageId = null)
        {
            if (chatRoomId is null)
            {
                throw new InvalidOperationException("chatroomId does not exist");
            }

            IQueryable<Message> messagesQuery = _dbContext.Messages.Where(message => message.ChatRoomId == chatRoomId).OrderByDescending(m => m.CreatedTime);
            if (string.IsNullOrEmpty(lastMessageId))
            {
                messagesQuery = messagesQuery.Take(pageSize!.Value);
            }
            else
            {
                var lastMessageTime = await _dbContext.Messages.Where(message => message.Id == lastMessageId).Select(message => message.CreatedTime).FirstOrDefaultAsync();
                messagesQuery = messagesQuery.Where(message => message.CreatedTime <= lastMessageTime && message.Id != lastMessageId).Take(pageSize!.Value);
            }
            messagesQuery = messagesQuery.Include(message => message.Sender);
            return await messagesQuery.ToListAsync();
        }
        public async Task<Message?> UpdateMessage(string Id, Message request)
        {
            var message = await GetMessageById(Id);
            if (message == null) return null;

            _dbContext.Entry(message).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            return message;
        }
    }
}
