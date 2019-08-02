using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Context
{
    public sealed partial class AppDbContext
    {
        // see @ https://trailheadtechnology.com/entity-framework-core-2-1-automate-all-that-boring-boiler-plate/
        private void InspectBeforeSave()
        {
            this.CheckReadOnlyEntries();

            var timestamp = DateTime.UtcNow;

            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is ISoftDelete)
                {
                    this.SetSoftDeleteEntity(entry);
                }

                if (entry.Entity is ITracked)
                {
                    this.SetTrackedEntity(entry, timestamp);
                }
            }
        }

        private void SetSoftDeleteEntity(EntityEntry entry)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Property("IsDeleted").CurrentValue = true;
            }
        }

        private void SetTrackedEntity(EntityEntry entry, DateTime timestamp)
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                entry.CurrentValues["ModifiedOn"] = timestamp;
                entry.CurrentValues["ModifiedById"] = this.currentSession.UserId;
            }

            if (entry.State == EntityState.Added)
            {
                entry.CurrentValues["CreatedOn"] = timestamp;
                entry.CurrentValues["CreatedById"] = this.currentSession.UserId;

                if (entry.Entity is ITenant)
                {
                    entry.Property("TenantId").CurrentValue = this.currentSession.TenantId;
                }
            }
        }

        private void CheckReadOnlyEntries()
        {
            if (this.ChangeTracker.Entries<IReadOnly>().Any(entry =>
                entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
            {
                throw new ReadOnlyException("Fatal Error: Attempt to change read-only data!");
            }
        }
    }
}
