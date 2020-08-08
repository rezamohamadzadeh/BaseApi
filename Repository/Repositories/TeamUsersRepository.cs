using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TeamUsersRepository : GenericRepositori<Tb_TeamUsers>, ITeamUsersRepository
    {
        public TeamUsersRepository(ApplicationDbContext Db) : base(Db)
        { }

        /// <summary>
        /// Check if anyone user be a member in other team
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<Tb_TeamUsers> CheckUserExistsInOtherGroup(string[] users, string teamId = null)
        {
            IEnumerable<Tb_TeamUsers> teamUsers = null;

            foreach (var item in users)
            {
                if (item != null)
                {
                    if (teamId != null)
                        teamUsers = await GetAsync(d => d.UserId == item && d.TeamId.ToString() != teamId, null, "User");
                    else
                        teamUsers = await GetAsync(d => d.UserId == item, null, "User");

                    if (teamUsers.Count() != 0)
                    {
                        var user = teamUsers.FirstOrDefault(d => d.UserId == item);
                        return user;
                    }

                }
            }

            return null;
        }

        public IEnumerable<string> GetUserTeamsForEdit(Guid teamID)
        {
            var users =
                Get(d => d.TeamId == teamID, d=> d.OrderBy(m => m.TeamId),"User").Select(s => s.User.Id);
            return users;
        }

        /// <summary>
        /// Check selected leader join to team
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool CheckLeaderUserInTeam(string[] users, string leaderId = null)
        {

            foreach (var item in users)
            {
                if (item == leaderId)
                    return true;
            }

            return false;
        }

        public async Task<IEnumerable<Tb_TeamUsers>> GetTeamUsersList(Guid teamId)
        {
            var teamUsers = GetAsync(d => d.TeamId == teamId, null, "Team,User");

            return await teamUsers;
        }
    }
}
