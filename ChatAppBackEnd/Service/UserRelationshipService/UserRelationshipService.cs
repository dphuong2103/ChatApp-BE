using AutoMapper;
using ChatAppBackEnd.Data;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.Relationships;
using ChatAppBackEnd.Service.HubService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Service.UserRelationshipService
{
    public class UserRelationshipService : IUserRelationshipService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHubService _hubService;

        public static string RelationshipUpdateType_Add = "Add";
        public static string RelationshipUpdateType_Update = "Update";
        public static string RelationshipUpdateType_Delete = "Delete";
        public UserRelationshipService(DataContext dbContext, IMapper mapper, IHubService hubService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hubService = hubService;
        }

        public async Task<UserRelationship> AddUserRelationship(NewUserRelationship request)
        {
            var userRelationship = _mapper.Map<UserRelationship>(request);

            _dbContext.UserRelationships.Add(userRelationship);

            await _dbContext.SaveChangesAsync();

            userRelationship = await _dbContext.UserRelationships
                .Include(ur => ur.TargetUser)
                .Include(ur => ur.InitiatorUser)
                .FirstOrDefaultAsync(ur => ur.Id == userRelationship.Id);

            await _hubService.SendRelationship(userRelationship!, RelationshipUpdateType_Add);
            return userRelationship!;
        }

        public async Task<UserRelationship?> DeleteRelationship(string relationshipId)
        {
            var relationship = _dbContext.UserRelationships.Find(relationshipId);
            if (relationship != null)
            {
                _dbContext.UserRelationships.Remove(relationship);
                await _dbContext.SaveChangesAsync();
                await _hubService.SendRelationship(relationship, RelationshipUpdateType_Delete);
            }
            return relationship;
        }

        public async Task<List<UserRelationship>?> GetUserRelationshipsByUserId(string userId)
        {
            var userRelationships = await _dbContext.UserRelationships
                .Include(userRelationship => userRelationship.TargetUser)
                .Include(userRelationship => userRelationship.InitiatorUser)
                .Where(ur => ur.InitiatorUserId == userId || ur.TargetUserId == userId).ToListAsync();
            return userRelationships;
        }

        public async Task<UserRelationship?> UpdateUserRelationship(string userRelationshipId, UserRelationship request)
        {
            var userRelationship = await _dbContext.UserRelationships.FindAsync(userRelationshipId);
            if (userRelationship is null) return default;
            request.UpdatedTime = DateTime.UtcNow;
            _dbContext.Entry(userRelationship).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            await _hubService.SendRelationship(userRelationship, RelationshipUpdateType_Update);
            return userRelationship;
        }

        public async Task<UserRelationship> SendFriendRequest(NewUserRelationship request)
        {
            var userRelationship = _mapper.Map<UserRelationship>(request);
            userRelationship.UpdatedTime = userRelationship.CreatedTime;
            await _dbContext.UserRelationships.AddAsync(userRelationship);
            await _dbContext.SaveChangesAsync();
            userRelationship = await _dbContext.UserRelationships.Include(userRelationship => userRelationship.TargetUser)
                .Include(userRelationship => userRelationship.InitiatorUser).FirstOrDefaultAsync(userRelationship => userRelationship.Id == userRelationship.Id);
            if (userRelationship is not null) await _hubService.SendRelationship(userRelationship, RelationshipUpdateType_Add);
            return userRelationship!;
        }

        public async Task<UserRelationship?> AcceptFriendRequest(string userRelationshipId, UserRelationship request)
        {
            var userRelationship = await _dbContext.UserRelationships.FindAsync(userRelationshipId);
            if (userRelationship is null) return default;
            userRelationship.Status = UserRelationship.RelationshipStatus_Accepted;
            userRelationship.RelationshipType = UserRelationship.RelationshipType_Friend;
            userRelationship.UpdatedTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            userRelationship = await _dbContext.UserRelationships.Include(userRelationship => userRelationship.TargetUser)
                .Include(userRelationship => userRelationship.InitiatorUser).FirstOrDefaultAsync(userRelationship => userRelationship.Id == userRelationshipId);
            if (userRelationship is not null) await _hubService.SendRelationship(userRelationship, RelationshipUpdateType_Update);
            return userRelationship;
        }
    }
}
