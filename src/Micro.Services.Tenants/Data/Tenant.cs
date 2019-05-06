using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class Tenant
    {
        public Tenant()
        {
            Roles = new List<Role>();
            Teams = new List<Team>();
            Users = new List<User>();
        }

        [Key]
        [Required()]
        public virtual int Id
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required()]
        public virtual string Name
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required()]
        public virtual string Host
        {
            get;
            set;
        }

        public virtual IList<Role> Roles
        {
            get;
            set;
        }

        public virtual IList<Team> Teams
        {
            get;
            set;
        }

        public virtual IList<User> Users
        {
            get;
            set;
        }
    }
}