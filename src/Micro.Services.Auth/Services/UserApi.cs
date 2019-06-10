using Micro.Services.Auth.Data;
using Micro.Services.Auth.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Services.Auth.Services
{
    public interface IUserApi
    {
        Task<User> AuthAsync(string email, string password);
        Task<User> GetUserAsync(int id);
        Task<string[]> GetUserPermissions(int id);
    }

    public class UserApi : IUserApi
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<UserApi> _log;

        public UserApi(DatabaseContext db, ILogger<UserApi> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<User> AuthAsync(string email, string password)
        {
            var user = await _db
                .Users
                .SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            if (user.Password != password)
            {
                return null;
            }
            return user;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _db
.Users
.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string[]> GetUserPermissions(int id)
        {
            return await _db.UserPermissions
.Where(x => x.UserId == id)
.Select(x => x.Name)
.ToArrayAsync();
        }
    }
}
