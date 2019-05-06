using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Tenants.Data
{
    public class Data : ITenantData
    {
        [Key]
        [Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual int TenantId { get; set; }

        public virtual Tenant Tenant
        {
            get;
            set;
        }
    }
}