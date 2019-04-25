using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _map;

        public TenantsController(DatabaseContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        // GET api/tenants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantModel>>> GetAsync()
        {
            var tenants = await _db.Tenants.ToListAsync();
            var models = _map.Map<IEnumerable<TenantModel>>(tenants);
            return Ok(models);
        }

        // GET api/tenants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantModel>> GetAsync(int id)
        {
            var tenant = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                return NotFound();
            }
            var model = _map.Map<TenantModel>(tenant);
            return Ok(model);
        }

        // POST api/tenants
        [HttpPost]
        public async Task<ActionResult<TenantModel>> PostAsync([FromBody] string name)
        {
            var exists = await _db.Tenants.AnyAsync(x => x.Name == name);
            if (exists)
            {
                return BadRequest();
            }
            var data = new TenantData { Name = name };
            await _db.Tenants.AddAsync(data);
            await _db.SaveChangesAsync();

            var tenant = await _db.Tenants.SingleAsync(x => x.Id == data.Id);
            var model = _map.Map<TenantModel>(tenant);
            return Ok(model);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TenantModel>> PutAsync(int id, [FromBody] string name)
        {
            var tenant = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                return NotFound();
            }
            tenant.Name = name;
            await _db.Tenants.AddAsync(tenant);
            await _db.SaveChangesAsync();

            var model = _map.Map<TenantModel>(tenant);
            return Ok(model);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var tenant = await _db.Tenants.SingleOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                return Ok();
            }

            _db.Tenants.Remove(tenant);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}

