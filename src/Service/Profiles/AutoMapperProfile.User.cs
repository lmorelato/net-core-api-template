using AutoMapper;
using Template.Core.Models.Dtos;
using Template.Data.Entities.Identity;

namespace Template.Core.Profiles
{
    public partial class AutoMapperProfile : Profile
    {
        private void MapUser()
        {
            this.CreateMap<User, UserDto>();
        }
    }
}