using GameStore.BLL.DTO.UserViews;
using GameStore.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers.AuthControllers {
    [Route("/users")]
    [ApiController]
   // [Authorize(PermissionEnum.Genre)]
    public class UserController : ControllerBase {

        private readonly UserService _userService;
        public UserController(UserService userService) {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest loginRequest) {

            var processLoginRequest = await _userService.ProcessLoginRequestAsync(loginRequest);
            if (processLoginRequest == string.Empty) {
                return BadRequest("Couldnt process your request");
            } else return Ok(new { token = processLoginRequest });

        }

        [HttpPost("access")]
        public async Task<IActionResult> GetUserAccess([FromBody] UserAccessRequest userAccessRequest) {
            return Ok(true);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUsersModel>>> GetAllUsers() {

            var getAllUsers = await _userService.GetAllUsersAsync();
            return Ok(getAllUsers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUsersModel>> GetUserById(string id) {
            var getUserById = await _userService.GetUserByIdAsync(id);
            return Ok(getUserById);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserById(string id) {
            return await _userService.DeleteUserByIdAsync(id) ? Ok("Succesfully deleted") : BadRequest("User id doesnt exist");
            
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] RegisterDTO registerRequest) {
            await _userService.AddUserAsync(registerRequest);
            return Ok(registerRequest);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserRequest) {
            await _userService.UpdateUserAsync(updateUserRequest);
            return Ok();
        }
        [HttpGet("{id}/roles")]
        public async Task<ActionResult<IEnumerable<GetRolesModel>>> GetAllUsersRoles(string id) {
            var allUsersRoles = await _userService.GetAllUsersRolesAsync(id);
            return Ok(allUsersRoles);
        }
    
    }
}
