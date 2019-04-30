using AutoMapper;
using Micro.Services.Tenants.Data;

namespace Micro.Services.Tenants.Models
{
    public class TenantModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TenantModelMappings : Profile
    {
        public TenantModelMappings()
        {
            CreateMap<TenantData, TenantModel>();
        }
    }

    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }
    }
}
