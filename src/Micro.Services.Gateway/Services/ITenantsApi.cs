using System.Collections.Generic;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public interface ITenantsApi
    {
        UserModel CreateUser(UserModel user);
        TeamModel CreateTeam(TeamModel team);

        Task<UserModel> GetUserAsync(string id);
        Task<TeamModel> GetTeamAsync(string id);
        Task<RoleModel> GetRoleAsync(string id);

        Task<IEnumerable<TeamModel>> ListTeamsAsync();
        Task<IEnumerable<UserModel>> ListUsersAsync();
        Task<IEnumerable<RoleModel>> ListRolesAsync();
        
        Task<IEnumerable<UserModel>> ListUsersByTeamAsync(string id);
        Task<IEnumerable<TeamModel>> ListTeamsByUserAsync(string id);
        Task<IEnumerable<RoleModel>> ListRolesByTeamAsync(string id);
        Task<IEnumerable<TeamModel>> ListTeamsByRoleAsync(string id);

        Task<IEnumerable<string>> ListPermissionsByRoleAsync(string id);
    }
}