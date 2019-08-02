using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Core.Exceptions;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Data.Context;
using Template.Data.Entities.Identity;
using Template.Localization;

namespace Template.Core.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly ISharedResources localizer;
        private readonly IMapper mapper;

        public UserService(
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ISharedResources localizer,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.localizer = localizer;
            this.mapper = mapper;
        }

        public async Task<UserDto> AddAsync(CredentialsDto credentials)
        {
            var newUser = new User
            {
                UserName = credentials.UserName,
                Email = credentials.UserName,
                Culture = "en-US"
            };

            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await this.userManager.CreateAsync(newUser, credentials.Password);
                    this.ThrowIfNotSucceed(result);
                    transaction.Commit();
                }
                catch (IdentityResultException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return this.mapper.Map<UserDto>(newUser);
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                var message = this.localizer.GetAndApplyKeys("NotFound", "User");
                throw new NotFoundException(message);
            }

            return this.mapper.Map<UserDto>(user);
        }

        private async Task AddToRoleAsync(User user, string role)
        {
            IdentityResult result;

            if (!await this.roleManager.RoleExistsAsync(role))
            {
                result = await this.roleManager.CreateAsync(new Role(role));
                this.ThrowIfNotSucceed(result);
            }

            result = await this.userManager.AddToRoleAsync(user, role);
            this.ThrowIfNotSucceed(result);
        }

        private void ThrowIfNotSucceed(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result);
            }
        }
    }
}
