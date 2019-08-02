using System.Net;
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

        public UsersController(
            IUserService userService,
            ICurrentSession currentSession) : base(currentSession)
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
        public async Task<ActionResult<UserDto>> GetAsync([FromRoute]int id)
        {
            var result = await this.userService.GetAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Adds a new user
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
    }
}


