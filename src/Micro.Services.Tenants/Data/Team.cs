using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class Team : Data
    {
        public Team()
        {
            TeamRoles = new List<TeamRole>();
            UserTeams = new List<UserTeam>();
        }

        [StringLength(255)]
        [Required]
        public virtual string Name
        {
            get;
            set;
        }

        public virtual IList<TeamRole> TeamRoles
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