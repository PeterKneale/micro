using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Auth.Data
{
    public class User : Data
    {
        [StringLength(255)]
        [Required()]
        public virtual string Email
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required()]
        public virtual string Password
        {
            get;
            set;
        }

        [Required]
        public virtual int TenantId { get; set; }
    }
}