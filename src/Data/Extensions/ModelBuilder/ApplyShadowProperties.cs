using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Template.Data.Entities.Identity;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Extensions.ModelBuilder
{
    public static partial class ModelBuilderExtensions
    {
        // see @ https://trailheadtechnology.com/entity-framework-core-2-1-automate-all-that-boring-boiler-plate/
        public static void ApplyShadowProperties(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(ITracked).IsAssignableFrom(clrType))
                {
                    InvokeMethod(modelBuilder, clrType, nameof(SetAuditingShadowProperties));
                }

                if (typeof(ISoftDelete).IsAssignableFrom(clrType))
                {
                    InvokeMethod(modelBuilder, clrType, nameof(SetIsDeletedShadowProperty));
                }
            }
        }

        private static void SetAuditingShadowProperties<T>(Microsoft.EntityFrameworkCore.ModelBuilder builder) where T : class, ITracked
        {
            builder.Entity<T>().Property<DateTime>("CreatedOn").HasDefaultValueSql("GetUtcDate()");
            builder.Entity<T>().Property<DateTime>("ModifiedOn").HasDefaultValueSql("GetUtcDate()");

            builder.Entity<T>().Property<int>("CreatedById");
            builder.Entity<T>().Property<int>("ModifiedById");

            builder.Entity<T>().HasOne<User>().WithMany().HasForeignKey("CreatedById").OnDelete(DeleteBehavior.Restrict);
            builder.Entity<T>().HasOne<User>().WithMany().HasForeignKey("ModifiedById").OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetIsDeletedShadowProperty<T>(Microsoft.EntityFrameworkCore.ModelBuilder builder) where T : class, ISoftDelete
        {
            builder.Entity<T>().Property<bool>("IsDeleted");
        }

        private static void InvokeMethod(object obj, Type type, string methodName)
        {
            var method = GetMethodInfo(methodName).MakeGenericMethod(type);
            method.Invoke(obj, new[] { obj });
        }

        private static MethodInfo GetMethodInfo(string methodName)
        {
            return typeof(ModelBuilderExtensions)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && (t.Name == methodName));
        }
    }
}
