using AutoMapper;
using ChatAppBackEnd.Data;
using ChatAppBackEnd.Exceptions;
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
            message.MessageText = message.MessageText.Trim();
            message.CreatedTime = DateTime.UtcNow;
            if (string.IsNullOrEmpty(message.MessageText))
            {
                throw new EmptyMessageException();
            }
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
            message = _dbContext.Messages.Include(m => m.Sender).FirstOrDefault(m => m.Id == message.Id);
            await _hubservice.SendMessage(message!);
            return message!;
        }

        public async Task<Message> SetDeleteMessage(string messageId)
        {
            var message = _dbContext.Messages.Include(message => message.Sender).FirstOrDefault(message => message.Id == messageId);
            if (message is null)
            {
                throw new NotFoundMessageException(messageId);
            }
            message.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            await _hubservice.SendMessage(message);
            return message;
        }

        public async Task<Message?> GetMessageById(string Id)
        {
            var message = await _dbContext.Messages.Include(m => m.Sender).FirstOrDefaultAsync(m => m.Id == Id);
            return message;
        }

        public async Task<List<Message>?> GetMessagesByChatRoomId(string chatRoomId)
        {
            var messagesByChatRoomId = await _dbContext.Messages.Where(message => message.ChatRoomId == chatRoomId && message.IsDeleted == false)
                .Include(message => message.Sender).OrderByDescending(message => message.CreatedTime)
                .ToListAsync();
            return messagesByChatRoomId;
        }

        public async Task<List<Message>?> GetMessagesPageByChatRoomId(string? chatRoomId, int? pageSize = 2, string? lastMessageId = null)
        {
            if (chatRoomId is null)
            {
                throw new Exception("chatroomId does not exist");
            }

            IQueryable<Message> messagesQuery = _dbContext.Messages.Where(message => message.ChatRoomId == chatRoomId && message.IsDeleted == false).OrderByDescending(m => m.CreatedTime);
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
        public async Task<Message> UpdateMessage(string Id, Message request)
        {
            var message = await GetMessageById(Id);
            if (message == null)
            {
                throw new NotFoundMessageException(Id);
            }
            _dbContext.Entry(message).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<Message> AddNewMessageForFileUpload(NewMessage request)
        {
            var message = _mapper.Map<Message>(request);
            message.CreatedTime = DateTime.UtcNow;
            message.Type = MessageType.Files;
            message.FileStatus = FileStatus.InProgress;
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UpdateMessageOnFileUploadFinish(string messageId, string fileUrls)
        {
            var message = await GetMessageById(messageId);
            if (message == null) throw new Exception("Cannot find message with Id: " + messageId);
            if (string.IsNullOrEmpty(fileUrls)) throw new EmptyFileUrlException();
            message.FileUrls = fileUrls;
            message.FileStatus = FileStatus.Done;
            await _dbContext.SaveChangesAsync();
            message = await _dbContext.Messages.Include(m=>m.Sender).FirstOrDefaultAsync(m => m.Id == message.Id);
            await _hubservice.SendMessage(message!);
            return message!;
        }

        public async Task CancelUploadingMessageFile(string messageId)
        {
            var message = await GetMessageById(messageId);
            if (message is null) throw new NotFoundMessageException(messageId);
            message.FileStatus = FileStatus.Cancelled;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMissingMessages(string lastMessageId)
        {
            var message = await _dbContext.Messages.FindAsync(lastMessageId);
            if (message is null)
            {
                throw new NotFoundMessageException(lastMessageId);
            }
            var messages = await _dbContext.Messages
                .Where(m => m.CreatedTime >= message.CreatedTime && m.Id != message.Id && m.ChatRoomId == message.ChatRoomId)
                .Include(m => m.Sender)
                .ToListAsync();
            return messages ?? new List<Message>();
        }

        public async Task<Message> AddNewMessageForAudioRecord(NewMessage request)
        {
            var message = _mapper.Map<Message>(request);
            message.CreatedTime = DateTime.UtcNow;
            message.Type = MessageType.AudioRecord;
            message.FileStatus = FileStatus.InProgress;
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UploadMessageFileError(string messageId)
        {
            var message = await _dbContext.Messages.FindAsync(messageId);

            if (message is null)
            {
                throw new NotFoundMessageException(messageId);
            }
            message.FileStatus = FileStatus.Error;
            await _dbContext.SaveChangesAsync();
            return message;
        }
    }
}
