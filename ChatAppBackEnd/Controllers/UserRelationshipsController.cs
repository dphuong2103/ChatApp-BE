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
            try
            {
                var userRelationship = await _userRelationshipService.AddUserRelationship(request);
                return Ok(userRelationship);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpGet("userid/{userId}")]
        public async Task<ActionResult<List<UserRelationship>?>> GetUserRelationshipsByUserId(string userId)
        {
            try
            {
                var userRelationships = await _userRelationshipService.GetUserRelationshipsByUserId(userId);
                return Ok(userRelationships);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }


        [HttpDelete("{relationshipId}")]
        public async Task<ActionResult<UserRelationship?>> DeleteRelationship(string relationshipId)
        {
            try
            {
                var userRelatiopnship =  await _userRelationshipService.DeleteRelationship(relationshipId);
                return Ok(userRelatiopnship);
            }
            catch (Exception err) {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{userRelationshipId}/accept")]
        public async Task<ActionResult> AcceptFriendRequest(string userRelationshipId, UserRelationship userRelationship)
        {
            try
            {
                await _userRelationshipService.AcceptFriendRequest(userRelationshipId, userRelationship);
                return NoContent();
            }
            catch (Exception err) {
                Console.WriteLine(err.Message);
                return BadRequest(err);
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserRelationship?>> UpdateUserRelationship(string userRelationshipId, UserRelationship request)
        {
            try
            {
                var userRelationship = await _userRelationshipService.UpdateUserRelationship(userRelationshipId, request);
                return NoContent();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err.Message);
            }
        }

    }
}
