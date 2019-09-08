using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Core.Exceptions;
using Template.Core.Helpers;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Data.Context;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IEmailService emailService;
        private readonly ISharedResources localizer;
        private readonly IMapper mapper;

        public UserService(
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IEmailService emailService,
            ISharedResources localizer,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.localizer = localizer;
            this.mapper = mapper;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await this.FindAsync(id);
            return this.mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(CredentialsDto credentials)
        {
            await this.emailService.SendTemplate();

            var newUser = this.mapper.Map<User>(credentials);
            newUser.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await this.userManager.CreateAsync(newUser, credentials.Password);
                    this.ThrowIfNotSucceed(result);
                    await this.AddToRoleAsync(newUser, Constants.Roles.User);
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

        public async Task UpdateAsync(UserDto userDto)
        {
            var userEntry = await this.FindAsync(userDto.Id);
            userEntry.FullName = userDto.FullName;
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateCultureAsync(int userId)
        {
            var userEntry = await this.FindAsync(userId);
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int userId)
        {
            var userEntry = await this.FindAsync(userId);
            this.context.Users.Remove(userEntry);
            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        private async Task<User> FindAsync(int id)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }

            var message = this.localizer.GetAndApplyKeys("NotFound", "User");
            throw new NotFoundException(message);
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
