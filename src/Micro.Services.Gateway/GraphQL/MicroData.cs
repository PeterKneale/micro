using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;

namespace Micro.Services.Gateway.GraphQL
{
    public class MicroData : IMicroData
    {
        private readonly List<UserModel> _users = new List<UserModel>();
        private readonly List<TeamModel> _teams = new List<TeamModel>();

        public MicroData()
        {
            _users.Add(new UserModel
            {
                Id = "1",
                Name = "Luke"
            });
            _users.Add(new UserModel
            {
                Id = "2",
                Name = "Vader"
            });
            _teams.Add(new TeamModel
            {
                Id = "1",
                Name = "A"
            });
            _teams.Add(new TeamModel
            {
                Id = "2",
                Name = "B"
            });
        }

        public Task<UserModel> GetUserAsync(string id)
        {
            return Task.FromResult(_users.FirstOrDefault(h => h.Id == id));
        }
        public Task<TeamModel> GetTeamAsync(string id)
        {
            return Task.FromResult(_teams.FirstOrDefault(h => h.Id == id));
        }

        public Task<List<UserModel>> ListUsersAsync()
        {
            return Task.FromResult(_users);
        }

        public Task<List<TeamModel>> ListTeamsAsync()
        {
            return Task.FromResult(_teams);
        }

        public Task<List<TeamModel>> ListTeamsByUserAsync(string id)
        {
            return ListTeamsAsync();
        }

        public Task<List<UserModel>> ListUsersByTeamAsync(string id)
        {
            return ListUsersAsync();
        }

        public UserModel CreateUser(UserModel user)
        {
            user.Id = Guid.NewGuid().ToString();
            _users.Add(user);
            return user;
        }

        public TeamModel CreateTeam(TeamModel TeamModel)
        {
            TeamModel.Id = Guid.NewGuid().ToString();
            _teams.Add(TeamModel);
            return TeamModel;
        }
    }
}
