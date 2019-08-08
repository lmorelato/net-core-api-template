using Microsoft.EntityFrameworkCore;

namespace Template.Data.Context
{
    public sealed partial class AppDbContext
    {
        // public virtual DbSet<AccessLog> AccessLogs { get; set; }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            // builder.ApplyConfiguration(new AccessLogConfig());
        }
    }
}