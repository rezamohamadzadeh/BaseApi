using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class SupportRepository : GenericRepositori<Tb_Support>, ISupportRepository
    {
        public SupportRepository(ApplicationDbContext Db) : base(Db)
        { }

    }

}
