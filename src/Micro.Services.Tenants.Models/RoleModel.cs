namespace Micro.Services.Tenants.Models
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string[] Permissions { get; set; }
    }
}
