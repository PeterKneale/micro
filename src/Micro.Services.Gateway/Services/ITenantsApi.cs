using System.Collections.Generic;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public interface ITenantsApi
    {
        TeamModel CreateTeam(TeamModel team);
        UserModel CreateUser(UserModel user);

        Task<TeamModel> GetTeamAsync(string id);
        Task<UserModel> GetUserAsync(string id);

        Task<IEnumerable<TeamModel>> ListTeamsAsync();
        Task<IEnumerable<UserModel>> ListUsersAsync();

        Task<IEnumerable<TeamModel>> ListTeamsByUserAsync(string id);
        Task<IEnumerable<UserModel>> ListUsersByTeamAsync(string id);
    }
}