using System.Collections.Generic;

namespace Micro.Services.Gateway.Models
{
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Permissions { get; set; }

        public IEnumerable<TeamModel> Teams { get; set; }
    }
}
