using AutoMapper;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.Relationships;

namespace ChatAppBackEnd.Helper.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewChatRoom, ChatRoom>().ReverseMap();
            CreateMap<Message, NewMessage>().ReverseMap();
            CreateMap<NewUserRelationship, UserRelationship>().ReverseMap();
        }
    }
}
