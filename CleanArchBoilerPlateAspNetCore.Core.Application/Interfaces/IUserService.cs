using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> GetByIdAsync(string id);

        Task<UserDto> AddAsync(UserDto dto, ClaimsPrincipal user);

        Task DeleteByIdAsync(string id);

        //Task<UserDto> UpdatePatchAsync(string id, JsonPatchDocument<UserDto> patchDoc, ClaimsPrincipal user);

        Task<IEnumerable<RoleDto>> GetUserRolesAsync(string id);

        Task<RoleDto> AddRoleToUserAsync(string userId, int roleId);

        Task DeleteRoleToUserAsync(string userId, int roleId);
    }
}
