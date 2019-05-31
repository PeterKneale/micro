using System.Collections.Generic;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public interface IMicroData
    {
        TeamModel CreateTeam(TeamModel team);
        UserModel CreateUser(UserModel user);

        Task<TeamModel> GetTeamAsync(string id);
        Task<UserModel> GetUserAsync(string id);

        Task<List<TeamModel>> ListTeamsAsync();
        Task<List<UserModel>> ListUsersAsync();

        Task<List<TeamModel>> ListTeamsByUserAsync(string id);
        Task<List<UserModel>> ListUsersByTeamAsync(string id);
    }
}