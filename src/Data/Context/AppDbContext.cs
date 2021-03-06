﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Data.Entities;
using Template.Data.Entities.Identity;
using Template.Data.Extensions.ModelBuilder;
using Template.Shared.Session;

namespace Template.Data.Context
{
    public partial class AppDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IUserSession userSession;
        private bool ensureAutoHistory;

        public AppDbContext(DbContextOptions options, IUserSession userSession) : base(options)
        {
            this.userSession = userSession;
        }

        public void EnsureAudit()
        {
            this.ensureAutoHistory = true;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyIdentityConfiguration();
            builder.ApplyShadowProperties();
            builder.ApplyConventions();
            builder.EnableAutoHistory<AuditLog>(o => { o.ChangedMaxLength = 2048 * 2; });

            this.ApplyGlobalQueryFilters(builder);
            this.ApplyConfigurations(builder);
        }
    }
}