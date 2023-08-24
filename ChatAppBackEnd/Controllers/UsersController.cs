using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Service.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] User newUser)
        {
            var user = await _userService.AddUser(newUser);
            return Ok(user);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<User>> GetUserById(string Id)
        {
            var user = await _userService.GetUserById(Id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("search/{filterValue}")]
        public async Task<ActionResult<List<User>>> SearchUsersByEmail(string filterValue)
        {
            var filterUsers = await _userService.SearchUsersByEmail(filterValue);
            return Ok(filterUsers);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<User>> UpdateUser(string Id, [FromBody] User user)
        {
            var updatedUser = await _userService.UpdateUser(Id, user);
            return Ok(updatedUser);
        }

        [HttpPost("google")]
        public async Task<ActionResult> AddGoogleUser(User request)
        {
            await _userService.AddGoogleUser(request);
            return Ok();
        }
    }
}
