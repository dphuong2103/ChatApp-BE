namespace ChatAppBackEnd.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message) { }
    }

    public sealed class UserAlreadyExistsException : AlreadyExistsException
    {
        public UserAlreadyExistsException(string userId) : base($"User already exists, userId: {userId}")
        {
        }
    }

    public sealed class ChatRoomAlreadyExistsException : AlreadyExistsException
    {
        public ChatRoomAlreadyExistsException(string chatRoomId) : base($"chatroom already exists, chatRoomId: {chatRoomId}") { }
    }

    public sealed class UserChatRoomAlreadyExistsException : AlreadyExistsException
    {
        public UserChatRoomAlreadyExistsException(string userChatRoomId) : base($"userChatRoom already exists, userChatRoomId: {userChatRoomId}") { }
    }

}
