using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class Role : Data
    {
        public Role()
        {
            RolePermissions = new List<RolePermission>();
            TeamRoles = new List<TeamRole>();
        }

        [StringLength(255)]
        [Required]
        public virtual string Name
        {
            get;
            set;
        }

        public virtual IList<RolePermission> RolePermissions
        {
            get;
            set;
        }

        public virtual IList<TeamRole> TeamRoles
        {
            get;
            set;
        }
    }
}