using Microsoft.AspNetCore.Authentication;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role;
using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Services
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IUserService _userService;

        public ClaimsTransformer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //Retrieves the roles for the current user
            List<RoleDto> roles = new List<RoleDto>(await _userService.GetUserRolesAsync(principal.Identity.Name));

            //Add the roles as RoleClaim in the user Identity
            ClaimsIdentity ci = (ClaimsIdentity)principal.Identity;

            foreach (RoleDto role in roles)
            {
                Claim c = new Claim(ci.RoleClaimType, role.Name);
                ci.AddClaim(c);
            }

            return await Task.FromResult(principal);
        }
    }
}
