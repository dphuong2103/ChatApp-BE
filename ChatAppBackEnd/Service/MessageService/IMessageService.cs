using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;

namespace ChatAppBackEnd.Service.MessageService
{
    public interface IMessageService
    {
        Task<Message> AddMessage(NewMessage newMessage);
        Task<List<Message>?> GetMessagesByChatRoomId(string chatRoomId);
        Task<Message?> GetMessageById(string Id);
        Task<Message> UpdateMessage(string Id, Message message);
        Task<List<Message>?> GetMessagesPageByChatRoomId(string? chatRoomId, int? pagesize, string? lastMessageId = null);
        Task<Message> SetDeleteMessage(string messageId);

        Task<Message> AddNewMessageForFileUpload(NewMessage request);
        Task<Message> UpdateMessageOnFileUploadFinish(string messageId, string fileUrls);
        Task CancelUploadingMessageFile(string messageId);

        Task<List<Message>> GetMissingMessages(string lastMessageId);
        Task<Message> AddNewMessageForAudioRecord(NewMessage request);
        Task<Message> UploadMessageFileError(string messageId);
    }
}
