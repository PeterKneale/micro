using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Services;

namespace Micro.Services.Tenants.DataContext
{
    public static class ModelBuilderExtensions
    {
        public static void SetTenantIdOnSave(this DbContext db, ITenantContext context)
        {
            var entries = db.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                // only on adds
                if (entry.State != EntityState.Added)
                {
                    continue;
                }
                // only on tenant data
                if (!(entry.Entity is ITenantData data))
                {
                    continue;
                }
                data.TenantId = context.TenantId;
            }
        }
    }
}