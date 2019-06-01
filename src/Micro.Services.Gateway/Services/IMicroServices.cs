using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Teams = Micro.Services.Tenants.Domain.Teams;
using Users = Micro.Services.Tenants.Domain.Users;

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

    public class TenantsApi : ITenantsApi
    {
        private readonly HttpClient _api;
        private readonly ILogger<TenantsApi> _log;

        public TenantsApi(HttpClient api, ILogger<TenantsApi> log)
        {
            _api = api;
            _log = log;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            throw new System.NotImplementedException();
        }

        public UserModel CreateUser(UserModel user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TeamModel> GetTeamAsync(string id)
        {
            var result = await Get<Teams.Get.Response>($"/api/teams/{id}");
            return ConvertTeam(result.Team);
        }

        public async Task<UserModel> GetUserAsync(string id)
        {
            var result = await Get<Users.Get.Response>($"/api/users/{id}");
            return ConvertUser(result.User);
        }

        public async Task<IEnumerable<TeamModel>> ListTeamsAsync()
        {
            var response = await Get<Teams.List.Response>("/api/teams");
            return response.Teams.Select(ConvertTeam);
        }

        public async Task<IEnumerable<TeamModel>> ListTeamsByUserAsync(string id)
        {
            var response = await Get<Users.ListTeams.Response>($"/api/users/{id}/teams");
            return response.Teams.Select(x => ConvertTeam(x));
        }

        public async Task<IEnumerable<UserModel>> ListUsersAsync()
        {
            var response = await Get<Users.List.Response>($"/api/users");
            return response.Users.Select(ConvertUser);
        }

        public async Task<IEnumerable<UserModel>> ListUsersByTeamAsync(string id)
        {
            var response = await Get<Teams.ListUsers.Response>($"/api/teams/{id}/users");
            return response.Users.Select(ConvertUser);
        }

        private async Task<T> Get<T>(string url)
        {
            var response = await _api.GetAsync(url);
            var status = response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("Api {url} returned {status} {content}", url, status, content);
                response.EnsureSuccessStatusCode();
            }

            _log.LogInformation("Api {url} returned {status} {content}", url, status, content);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private static TeamModel ConvertTeam(Tenants.Models.TeamModel model) => new TeamModel
        {
            Id = model.Id.ToString(),
            Name = model.Name
        };

        private static UserModel ConvertUser(Tenants.Models.UserModel model) => new UserModel
        {
            Id = model.Id.ToString(),
            Name = model.FirstName + " " + model.LastName
        };
    }
}