using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Auth.Data
{
    public class UserPermission : Data
    {
        [Required]
        public virtual int UserId
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

        public virtual User User
        {
            get;
            set;
        }
    }
}