using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature;
using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.WebAPI.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Controllers
{
    public class FeaturesController : APIControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeaturesController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        /// <summary>
        /// Returns the specified feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[AuthorizeByRole(APIRoles.Admin, APIRoles.Editor, APIRoles.Reader)]
        public async Task<ActionResult<FeatureDto>> Get(int id)
        {
            return Ok(await _featureService.GetByIdAsync(id));
        }

        /// <summary>
        /// Return all the features
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeByRole(APIRoles.Admin, APIRoles.Editor, APIRoles.Reader)]
        public async Task<ActionResult<IEnumerable<FeatureDto>>> Get()
        {
            return Ok(await _featureService.GetAllAsync());
        }

        /// <summary>
        /// Return the possible status for a feature
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("status")]
        //[AuthorizeByRole(APIRoles.Admin, APIRoles.Editor, APIRoles.Reader)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeatureStatusDto>))]
        public async Task<IActionResult> GetStatus()
        {
            return Ok(await _featureService.GetStatus());
        }

        /// <summary>
        /// Create a feature
        /// </summary>
        /// <param name="dto">All the required information to create a new Feature.</param>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the model doesn't match the requirements of the DTO.</response>  
        /// <response code="401">If the User isn't authenticated.</response>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeByRole(APIRoles.Admin, APIRoles.Editor)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeatureDto>> Add(CreateFeatureDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeatureDto featureCreated = await _featureService.AddAsync(dto, User);

            return CreatedAtAction(nameof(Get), new { id = featureCreated.Id }, featureCreated);
        }

        /// <summary>
        /// Delete the specified feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
       // [AuthorizeByRole(APIRoles.Admin, APIRoles.Editor)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _featureService.DeleteByIdAsync(id);
            return Ok();
        }

        /// <summary>
        /// Update the specified feature partially with a JSON Patch (https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch]
       // [AuthorizeByRole(APIRoles.Admin, APIRoles.Editor)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] JsonPatchDocument<UpdateFeatureDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("The JSON patch object is required");
            }

            FeatureDto updatedDto = await _featureService.UpdatePatchAsync(id, patchDoc, User);

            return Ok(updatedDto);
        }
    }
}
