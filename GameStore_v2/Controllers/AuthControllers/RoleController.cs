using GameStore.BLL.DTO.UserViews;
using GameStore.BLL.Services;
using GameStore.WEB.AuthUtilities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers.AuthControllers {
    [Route("/roles")]
    [ApiController]
    public class RoleController : ControllerBase {

        private readonly RoleService _roleService;
        public RoleController(RoleService roleService) {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRolesModel>>> GetAllRoles() {
            var allRoles = await _roleService.GetAllRolesAsync();
            return Ok(allRoles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetRolesModel>> GetRolesById(string id) {
            var roleById = await _roleService.GetRoleByIdAsync(id);
            return Ok(roleById);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleById(string id) {
            var deleted = await _roleService.DeleteRoleByIdAsync(id);
            return deleted ? Ok("Role deleted") : new NotFoundResult();
        }

        [HttpGet("permissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllPermisions() {
            var allPermissions = await _roleService.GetAllPermissionAsync();
            return Ok(allPermissions);
        }

        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermisionsByRoleId(string id) {
            var allPermissions = await _roleService.GetAllPermissionByRoleIdAsync(id);
            return Ok(allPermissions);
        }
        [HasPremission(PermissionEnum.ManageRole)]
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] AddRoleRequest addRoleRequest) {
            await _roleService.AddRoleAsync(addRoleRequest);
            return Ok();
        }
        [HasPremission(PermissionEnum.ManageRole)]
        [HttpPut]
        public async Task<ActionResult> UpdateRole([FromBody] UpdateRoleRequest updateRoleRequest) {
            await _roleService.UpdateRoleAsync(updateRoleRequest);
            return Ok();
        }
    }
}
