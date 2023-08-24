using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.Relationships;
using ChatAppBackEnd.Service.UserRelationshipService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRelationshipsController : ControllerBase
    {
        private readonly IUserRelationshipService _userRelationshipService;
        public UserRelationshipsController(IUserRelationshipService userRelationshipService)
        {
            _userRelationshipService = userRelationshipService;
        }

        [HttpPost]
        public async Task<ActionResult<UserRelationship>> AddUserRelationship(NewUserRelationship request)
        {
            var userRelationship = await _userRelationshipService.AddUserRelationship(request);
            return Ok(userRelationship);
        }

        [HttpGet("userid/{userId}")]
        public async Task<ActionResult<List<UserRelationship>?>> GetUserRelationshipsByUserId(string userId)
        {
            var userRelationships = await _userRelationshipService.GetUserRelationshipsByUserId(userId);
            return Ok(userRelationships);
        }


        [HttpDelete("{relationshipId}")]
        public async Task<ActionResult<UserRelationship?>> DeleteRelationship(string relationshipId)
        {
            var userRelatiopnship = await _userRelationshipService.DeleteRelationship(relationshipId);
            return Ok(userRelatiopnship);
        }

        [HttpPut("{userRelationshipId}/accept")]
        public async Task<ActionResult> AcceptFriendRequest(string userRelationshipId, UserRelationship userRelationship)
        {
            await _userRelationshipService.AcceptFriendRequest(userRelationshipId, userRelationship);
            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserRelationship?>> UpdateUserRelationship(string userRelationshipId, UserRelationship request)
        {
            await _userRelationshipService.UpdateUserRelationship(userRelationshipId, request);
            return NoContent();
        }

    }
}
