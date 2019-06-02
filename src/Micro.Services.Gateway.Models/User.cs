using System.Collections.Generic;

namespace Micro.Services.Gateway.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public IEnumerable<TeamModel> Teams { get; set; }
    }
}
