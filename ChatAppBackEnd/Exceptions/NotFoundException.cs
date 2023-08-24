namespace ChatAppBackEnd.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public sealed class NotFoundChatRoomException : NotFoundException
    {
        public NotFoundChatRoomException(string chatRoomId) : base($"Cannot find chatroom with Id: {chatRoomId}")
        {

        }
    }

    public sealed class NotFoundUserChatRoomException : NotFoundException
    {

        public NotFoundUserChatRoomException(string message) : base(message) { }
        public static NotFoundUserChatRoomException ById(string Id)
        {
            return new NotFoundUserChatRoomException($"Cannot find userchatroom with Id: {Id}");
        }
        public static NotFoundUserChatRoomException ByChatRoomIdAndUserId(string userId, string chatRoomId)
        {
            return new NotFoundUserChatRoomException($"Cannot find userchatroom with UserId: {userId} & ChatRoomId: {chatRoomId}");
        }
    }

    public sealed class NotFoundMessageException : NotFoundException
    {
        public NotFoundMessageException(string messageId) : base($"Cannot find message with Id: {messageId}") { }
    }

    public sealed class NotFoundUserRelationshipException : NotFoundException
    {
        public NotFoundUserRelationshipException(string message) : base(message) { }
        public static NotFoundUserRelationshipException ById(string Id)
        {
            return new NotFoundUserRelationshipException($"Cannot find relationship with Id: {Id}");
        }
        public static NotFoundUserRelationshipException ByUserIds(string userId1, string userId2)
        {
            return new NotFoundUserRelationshipException($"Cannot find relationship with UserId1: {userId1} & UserId2: {userId2}");
        }
    }

    public sealed class NotFoundUserException : NotFoundException
    {
        public NotFoundUserException(string userId) : base(userId) { }
    }
}
