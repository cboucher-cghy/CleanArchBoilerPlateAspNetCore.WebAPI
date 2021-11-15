using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.User;
using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns the specified user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(string id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        /// <summary>
        /// Return all the users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            return Ok(await _userService.GetAllAsync());
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="dto">All the required information to create a new User.</param>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the model doesn't match the requirements of the DTO.</response>  
        /// <response code="401">If the User isn't authenticated.</response>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "CleanArchBoilerPlateAspNetCore Administrator")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Add(UserDto dto)
        {
            if (!(User != null && User.Identity.IsAuthenticated))
            {
                return StatusCode(401);
            }

            UserDto UserCreated = await _userService.AddAsync(dto, User);

            return CreatedAtAction(nameof(Get), new { id = UserCreated.Id }, UserCreated);
        }

        /// <summary>
        /// Delete the specified user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize(Roles = "CleanArchBoilerPlateAspNetCore Administrator")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!(User != null && User.Identity.IsAuthenticated))
            {
                return StatusCode(401);
            }

            await _userService.DeleteByIdAsync(id);
            return Ok();
        }

        /// <summary>
        /// Update the specified feature partially with a JSON Patch (https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch]
        //[Authorize(Roles = "CleanArchBoilerPlateAspNetCore Administrator")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] JsonPatchDocument<UserDto> patchDoc)
        {
            if (!(User != null && User.Identity.IsAuthenticated))
            {
                return StatusCode(401);
            }

            if (patchDoc == null)
            {
                return BadRequest();
            }

            //FeatureDto updatedDto = await _userService.UpdatePatchAsync(id, patchDoc, User);

            //return Ok(updatedDto);
            return Ok();
        }

        /// <summary>
        /// Return the roles associated to a specific user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRolesByUser(string id)
        {
            return Ok(await _userService.GetUserRolesAsync(id));
        }


        [HttpPost]
        [Route("{id}/roles/{roleId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleDto>> AddRoleToUser(string id, int roleId)
        {
            if (!(User != null && User.Identity.IsAuthenticated))
            {
                return StatusCode(401);
            }

            RoleDto roleAddedDto = await _userService.AddRoleToUserAsync(id, roleId);

            return Ok(roleAddedDto);
        }

        [HttpDelete]
        [Route("{id}/roles/{roleId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleDto>> DeleteRoleToUser(string id, int roleId)
        {
            if (!(User != null && User.Identity.IsAuthenticated))
            {
                return StatusCode(401);
            }

            await _userService.DeleteRoleToUserAsync(id, roleId);

            return Ok();
        }

    }
}
