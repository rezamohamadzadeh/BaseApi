using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class SupportTypeRepository : GenericRepositori<Tb_SupportType>, ISupportTypeRepository
    {
        public SupportTypeRepository(ApplicationDbContext Db) : base(Db)
        { }


    }

}
