using Microsoft.AspNetCore.JsonPatch;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces
{
    public interface IFeatureService
    {
        Task<IEnumerable<FeatureDto>> GetAllAsync();

        Task<FeatureDto> GetByIdAsync(int id);

        Task<FeatureDto> AddAsync(CreateFeatureDto dto, ClaimsPrincipal user);

        Task DeleteByIdAsync(int id);

        Task<FeatureDto> UpdatePatchAsync(int id, JsonPatchDocument<UpdateFeatureDto> patchDoc, ClaimsPrincipal user);

        Task<IEnumerable<FeatureStatusDto>> GetStatus();

    }
}
