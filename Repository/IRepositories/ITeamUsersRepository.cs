using DAL.Models;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ITeamUsersRepository : IGenericRepository<Tb_TeamUsers>
    {
        Task<Tb_TeamUsers> CheckUserExistsInOtherGroup(string[] users, string teamId = null);

        bool CheckLeaderUserInTeam(string[] users, string leaderId = null);

        Task<IEnumerable<Tb_TeamUsers>> GetTeamUsersList(Guid teamId);

        IEnumerable<string> GetUserTeamsForEdit(Guid teamID);

    }
}
