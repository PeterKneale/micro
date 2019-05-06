using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class TeamRole : Data
    {
        [Required]
        public virtual int RoleId { get; set; }

        [Required]
        public virtual int TeamId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Team Team { get; set; }
    }
}