using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class User : Data
    {
        public User()
        {
            UserTeams = new List<UserTeam>();
        }

        [StringLength(255)]
        [Required()]
        public virtual string FirstName
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required()]
        public virtual string LastName
        {
            get;
            set;
        }

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

        public virtual IList<UserTeam> UserTeams
        {
            get;
            set;
        }
    }
}