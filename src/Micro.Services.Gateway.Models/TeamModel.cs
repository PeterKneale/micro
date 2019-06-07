using System.Collections.Generic;

namespace Micro.Services.Gateway.Models
{
    public class TeamModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserModel> Users { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
