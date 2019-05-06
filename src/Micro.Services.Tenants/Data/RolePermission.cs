using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class RolePermission : Data
    {
        [Required]
        public virtual int RoleId
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required]
        public virtual string Name
        {
            get;
            set;
        }

        public virtual Role Role
        {
            get;
            set;
        }
    }
}