using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;
using Micro.Services.Tenants.Models.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Micro.Services.Gateway.GraphQL
{
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



        public async Task<UserModel> GetUserAsync(string id)
        {
            var result = await Get<UserModel>($"/api/users/{id}");
            return Convert(result);
        }

        public async Task<TeamModel> GetTeamAsync(string id)
        {
            var result = await Get<TeamModel>($"/api/teams/{id}");
            return Convert(result);
        }

        public async Task<RoleModel> GetRoleAsync(string id)
        {
            var result = await Get<RoleModel>($"/api/roles/{id}");
            return Convert(result);
        }



        public async Task<IEnumerable<UserModel>> ListUsersAsync()
        {
            var response = await Get<PagedResponse<UserModel>>($"/api/users");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<TeamModel>> ListTeamsAsync()
        {
            var response = await Get<PagedResponse<TeamModel>>("/api/teams");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<RoleModel>> ListRolesAsync()
        {
            var response = await Get<PagedResponse<RoleModel>>("/api/roles");
            return response.Items.Select(Convert);
        }




        public async Task<IEnumerable<UserModel>> ListUsersByTeamAsync(string id)
        {
            var response = await Get<PagedResponse<UserModel>>($"/api/teams/{id}/users");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<TeamModel>> ListTeamsByRoleAsync(string id)
        {
            var response = await Get<PagedResponse<TeamModel>>($"/api/roles/{id}/teams");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<TeamModel>> ListTeamsByUserAsync(string id)
        {
            var response = await Get<PagedResponse<TeamModel>>($"/api/users/{id}/teams");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<RoleModel>> ListRolesByTeamAsync(string id)
        {
            var response = await Get<PagedResponse<RoleModel>>($"/api/teams/{id}/roles");
            return response.Items.Select(Convert);
        }

        public async Task<IEnumerable<string>> ListPermissionsByRoleAsync(string id)
        {
            var response = await Get<string[]>($"/api/roles/{id}/permissions");
            return response;
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

        private static TeamModel Convert(TeamModel model) => new TeamModel
        {
            Id = model.Id.ToString(),
            Name = model.Name
        };

        private static UserModel Convert(UserModel model) => new UserModel
        {
            Id = model.Id.ToString(),
            Name = model.FirstName + " " + model.LastName
        };

        private static RoleModel Convert(RoleModel model) => new RoleModel
        {
            Id = model.Id.ToString(),
            Name = model.Name
        };
    }
}