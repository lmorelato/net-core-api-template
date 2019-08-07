using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Api.Controllers.Bases;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Shared.Session;

namespace Template.Api.Controllers
{
    public class UsersController : AuthControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService, IUserSession currentUser) : base(currentUser)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Find user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Found user</returns>
        /// <response code="200">Returns the found user</response>
        /// <response code="401">Unauthorized</response>    
        /// <response code="404">If not found the user</response>    
        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetAsync([FromRoute]int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            var result = await this.userService.GetAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>A newly created user</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the credentials are invalid</response>            
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> PostAsync([FromBody]CredentialsDto credentials)
        {
            var result = await this.userService.AddAsync(credentials);
            return this.CreatedAtAction(nameof(this.GetAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="userDto">User data</param>
        /// <returns>No content</returns>
        /// <response code="204">Returns no content</response>
        /// <response code="400">If the user data is invalid</response>
        /// <response code="401">Unauthorized</response> 
        /// <response code="404">If not found the user</response>     
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UserDto userDto)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            userDto.Id = id;
            await this.userService.UpdateAsync(userDto);
            return this.NoContent();
        }

        /// <summary>
        /// Update user's culture
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>No content</returns>
        /// <response code="204">Returns no content</response>
        /// <response code="400">If the user data is invalid</response>
        /// <response code="401">Unauthorized</response> 
        /// <response code="404">If not found the user</response>     
        [HttpPatch("{id:int}/culture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PatchCultureAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            await this.userService.UpdateCultureAsync(id);
            return this.NoContent();
        }

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        /// <response code="204">Returns no content</response>
        /// <response code="400">If the user data is invalid</response>    
        /// <response code="404">If not found the user</response>  
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            await this.userService.RemoveAsync(id);
            return this.NoContent();
        }
    }
}


