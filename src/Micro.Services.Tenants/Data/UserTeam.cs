using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class UserTeam : Data
    {
        [Required()]
        public virtual int UserId
        {
            get;
            set;
        }

        [Required()]
        public virtual int TeamId
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual Team Team
        {
            get;
            set;
        }
    }
}