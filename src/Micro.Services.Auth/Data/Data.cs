using System.ComponentModel.DataAnnotations;

namespace Micro.Services.Auth.Data
{
    public class Data
    {
        [Key]
        [Required]
        public virtual int Id { get; set; }
    }
}