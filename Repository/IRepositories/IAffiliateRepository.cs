using DAL.Models;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IAffiliateRepository : IGenericRepository<Tb_Affiliates>
    {
        Task<string> GetTopAffiliatesSell();
        Task<List<Tb_Affiliates>> GetAffiliates();
    }
}
