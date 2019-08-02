using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public TokensController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<TokenDto>> AuthenticateAsync([FromBody]CredentialsDto credentials)
        {
            var result = await this.tokenService.AuthenticateAsync(credentials);
            return result;
        }
    }
}