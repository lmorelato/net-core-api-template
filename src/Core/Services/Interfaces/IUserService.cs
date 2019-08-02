
using System.Threading.Tasks;
using Template.Core.Models.Dtos;

namespace Template.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddAsync(CredentialsDto credentials);

        Task<UserDto> GetAsync(int id);
    }
}
