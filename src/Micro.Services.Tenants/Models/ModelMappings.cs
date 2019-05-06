using AutoMapper;
using Micro.Services.Tenants.Data;

namespace Micro.Services.Tenants.Models
{
    public class ModelMappings : Profile
    {
        public ModelMappings()
        {
            CreateMap<Tenant, TenantModel>();
            CreateMap<User, UserModel>();
            CreateMap<Team, TeamModel>();
        }
    }
}
