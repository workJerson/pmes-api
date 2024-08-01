using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.entity.Context
{
    public partial class DatabaseContext
    {
        public Task<int> SaveChangesAsync()
        {
            UpdateAuditEntities();

            return base.SaveChangesAsync();
        }

        private void UpdateAuditEntities(dynamic type = null, string userName = null)
        {
            var username = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "cognito:username")?.Value;
            username ??= userName;

            // https://medium.com/@unhandlederror/deleting-it-softly-with-ef-core-5f191db5cf72
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));


            foreach (var entry in modifiedEntries)
            {
                dynamic entity = entry.Entity;

                var now = DateTime.UtcNow;
                if (entry.State != EntityState.Deleted)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = now;
                        entity.CreatedBy = username;
                        entity.Guid = Guid.NewGuid();
                    }
                    else
                    {
                        entry.Property("Guid").IsModified = false;
                        entry.Property("CreatedOn").IsModified = false;
                        entry.Property("CreatedBy").IsModified = false;
                        entity.UpdatedOn = now;
                        entity.UpdatedBy = username;
                    }
                }
                else
                {
                    if (type == null)
                    {
                        entry.State = EntityState.Modified;
                        dynamic entityDel = entry.Entity;
                        entityDel.IsActive = true;
                        entityDel.IsDeleted = true;
                        entityDel.DeletedOn = now;
                        entityDel.DeletedBy = username;
                    }
                }
            }
        }
    }
}
