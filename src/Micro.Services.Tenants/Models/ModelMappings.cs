using AutoMapper;
using Micro.Services.Tenants.Data;
using System.Linq;

namespace Micro.Services.Tenants.Models
{
    public class ModelMappings : Profile
    {
        public ModelMappings()
        {
            CreateMap<Tenant, TenantModel>();
            CreateMap<User, UserModel>();
            CreateMap<Team, TeamModel>();
            CreateMap<Role, RoleModel>()
                .ForMember(dst => dst.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(x => x.Name).ToArray()));
        }
    }
}
