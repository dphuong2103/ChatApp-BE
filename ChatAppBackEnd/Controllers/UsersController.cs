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
            try
            {
                var user = await _userService.AddUser(newUser);
                return Ok(user);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<User>> GetUserById(string Id)
        {
            try
            {
                var user = await _userService.GetUserById(Id);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("search/{filterValue}")]
        public async Task<ActionResult<List<User>>> SearchUsersByEmail(string filterValue)
        {
            try
            {
                var filterUsers = await _userService.SearchUsersByEmail(filterValue);
                return Ok(filterUsers);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                var allUsers = await _userService.GetAllUsers();
                return Ok(allUsers);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<User>> UpdateUser(string Id, [FromBody] User user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(Id, user);
                return Ok(updatedUser);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
