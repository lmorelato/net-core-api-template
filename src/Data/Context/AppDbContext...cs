using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Data.Entities.Identity;
using Template.Data.Extensions.ModelBuilder;
using Template.Shared.Session;

namespace Template.Data.Context
{
    public sealed partial class AppDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly ICurrentSession currentSession;

        public AppDbContext(DbContextOptions options, ICurrentSession currentSession) : base(options)
        {
            this.currentSession = currentSession;
        }

        public override int SaveChanges()
        {
            this.InspectBeforeSave();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.InspectBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.InspectBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            this.InspectBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyIdentityConfiguration();
            builder.ApplyShadowProperties();
            this.ApplyGlobalQueryFilters(builder);
        }
    }
}