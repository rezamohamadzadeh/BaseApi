using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AffiliateRepository : GenericRepositori<Tb_Affiliates>, IAffiliateRepository
    {
        public AffiliateRepository(ApplicationDbContext Db) : base(Db)
        { }

        public async Task<List<Tb_Affiliates>> GetAffiliates()
        {
            var affiliates = await _context.Tb_Affiliates
                                     .Select(d => new Tb_Affiliates { FirstName = d.FirstName + " " + d.LastName, Code = d.Code })
                                     .OrderBy(d => d.FirstName).ToListAsync();

            return affiliates;
        }

        public async Task<string> GetTopAffiliatesSell()
        {
            var topSells = await _context
                .Tb_Sells.GroupBy(d => d.AffiliateCode)
                .Select(m => new { Count = m.Count(), Code = m.Key })
                .OrderByDescending(s => s.Count)
                .ToListAsync();

            return JsonConvert.SerializeObject(topSells);

        }

    }
}
